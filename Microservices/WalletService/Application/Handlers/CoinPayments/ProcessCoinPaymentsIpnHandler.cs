using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.Domain.Core.Events;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProductsRequests = Ecosystem.WalletService.Domain.Requests.WalletRequest.ProductsRequests;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class ProcessCoinPaymentsIpnHandler : IRequestHandler<ProcessCoinPaymentsIpnCommand, bool>
{
    private readonly ICoinPaymentsAdapter _adapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IProductValidationService _productValidator;
    private readonly IExternalPaymentStrategy _paymentStrategy;
    private readonly ICacheService _cacheService;
    private readonly IEventBus _eventBus;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationConfiguration _appSettings;
    private readonly ILogger<ProcessCoinPaymentsIpnHandler> _logger;

    public ProcessCoinPaymentsIpnHandler(
        ICoinPaymentsAdapter adapter,
        ITransactionRepository transactionRepository,
        IInvoiceRepository invoiceRepository,
        IProductValidationService productValidator,
        IExternalPaymentStrategy paymentStrategy,
        ICacheService cacheService,
        IEventBus eventBus,
        IUnitOfWork unitOfWork,
        IOptions<ApplicationConfiguration> appSettings,
        ILogger<ProcessCoinPaymentsIpnHandler> logger)
    {
        _adapter = adapter;
        _transactionRepository = transactionRepository;
        _invoiceRepository = invoiceRepository;
        _productValidator = productValidator;
        _paymentStrategy = paymentStrategy;
        _cacheService = cacheService;
        _eventBus = eventBus;
        _unitOfWork = unitOfWork;
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessCoinPaymentsIpnCommand command, CancellationToken cancellationToken)
    {
        var ipn = command.Request;

        if (!IsRequestValid(command))
        {
            _logger.LogWarning("Rejected CoinPayments IPN for txn {TxnId}: invalid request", ipn.txn_id);
            return false;
        }

        var transaction = await _transactionRepository.GetTransactionByTxnId(ipn.txn_id);
        if (transaction is null)
        {
            _logger.LogWarning("No transaction found for CoinPayments txn {TxnId}", ipn.txn_id);
            return false;
        }

        // Idempotency: a completed transaction is never touched again, no matter how many times
        // the provider redelivers the notification.
        if (transaction.Status == Constants.CompletedStatusCode)
        {
            _logger.LogInformation("CoinPayments txn {TxnId} was already completed", ipn.txn_id);
            return false;
        }

        // The IPN is anonymous, so the brand comes from the stored transaction rather than from
        // the (unresolved) tenant context.
        var brandId = transaction.BrandId;

        transaction.Status = ipn.status;
        transaction.AmountReceived = ipn.received_amount;
        transaction.UpdatedAt = DateTime.UtcNow;

        if (ipn.status == Constants.ExpiredStatusCode)
        {
            await HandleCancelled(transaction, brandId);
            return false;
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var credited = false;

            if (!transaction.Acredited)
                credited = await CreditPurchase(transaction, ipn, brandId);

            await _transactionRepository.UpdateTransactionAsync(transaction);
            await _unitOfWork.CommitAsync();

            if (credited)
                await _cacheService.InvalidateBalanceAsync(transaction.AffiliateId);

            _logger.LogInformation(
                "CoinPayments IPN processed for txn {TxnId} with status {Status}", ipn.txn_id, ipn.status);

            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Failed to process the CoinPayments IPN for txn {TxnId}", ipn.txn_id);
            return false;
        }
    }

    /// <summary>
    /// Reproduces the provider's own envelope checks. The HMAC comparison stays behind
    /// <c>RequireIpnSignature</c>: the old service never validated the digest, so the configured
    /// IpnSecret has never been proven against live traffic and enabling it blindly would reject
    /// every legitimate payment.
    /// </summary>
    private bool IsRequestValid(ProcessCoinPaymentsIpnCommand command)
    {
        var ipn = command.Request;

        if (!string.Equals(ipn.ipn_mode, CoinPaymentsConstants.IpnModeHmac, StringComparison.OrdinalIgnoreCase))
            return false;

        if (!command.Headers.TryGetValue(CoinPaymentsConstants.SignatureHeader, out var signature)
            || string.IsNullOrEmpty(signature))
            return false;

        if (string.IsNullOrEmpty(ipn.merchant))
            return false;

        if (ipn.ipn_type != CoinPaymentsConstants.IpnTypeApi)
            return false;

        if (!CoinPaymentsConstants.ValidCurrencies.Contains(ipn.currency1))
            return false;

        if (_appSettings.ConPayments?.RequireIpnSignature != true)
            return true;

        if (!string.Equals(ipn.merchant, _appSettings.ConPayments.MerchantId, StringComparison.Ordinal))
        {
            _logger.LogWarning("CoinPayments IPN for txn {TxnId} carries an unknown merchant", ipn.txn_id);
            return false;
        }

        if (_adapter.VerifyIpnSignature(command.RawBody, signature))
            return true;

        _logger.LogWarning("CoinPayments IPN for txn {TxnId} failed HMAC verification", ipn.txn_id);
        return false;
    }

    /// <summary>
    /// Undoes a purchase the buyer never completed: the invoice is rolled back through the stored
    /// procedure and, for memberships, the affiliate's activation is reverted.
    /// </summary>
    private async Task HandleCancelled(Transaction transaction, long brandId)
    {
        transaction.Acredited = false;
        await _transactionRepository.UpdateTransactionAsync(transaction);

        var invoice = await _invoiceRepository.GetInvoiceByReceiptNumber(transaction.IdTransaction, brandId);
        if (invoice is null)
        {
            _logger.LogInformation(
                "CoinPayments txn {TxnId} was cancelled before an invoice existed", transaction.IdTransaction);
            return;
        }

        var products = ReadProducts(transaction);

        if (products.Any(p => p.ProductId == CoinPaymentsConstants.MembershipProductId))
            await _eventBus.Publish(new RevertActivationDateEvent(invoice.AffiliateId, brandId));

        await _invoiceRepository.RevertCoinPaymentTransactions(
            [new InvoiceNumber { InvoiceNumberValue = invoice.Id }]);

        _logger.LogInformation("Reverted CoinPayments txn {TxnId}", transaction.IdTransaction);
    }

    /// <summary>Credits the purchase once funds arrived. Returns whether the wallet was touched.</summary>
    private async Task<bool> CreditPurchase(Transaction transaction, IpnRequest ipn, long brandId)
    {
        // Funds received (1) and complete (100) both credit, matching the old service: the
        // provider considers the payment good from the first confirmation onwards.
        if (ipn.status is not (CoinPaymentsConstants.FundsReceivedStatusCode or Constants.CompletedStatusCode))
            return false;

        // No amount comparison here, deliberately: received_amount is an int and the stored amount
        // carries the 1% surcharge, so any such check would reject correct payments.
        if (await _invoiceRepository.InvoiceExistsByReceiptNumber(transaction.IdTransaction, brandId))
        {
            _logger.LogInformation(
                "Invoice already exists for CoinPayments txn {TxnId}", transaction.IdTransaction);
            return false;
        }

        var products = ReadProducts(transaction);
        if (products.Count == 0)
        {
            _logger.LogWarning("CoinPayments txn {TxnId} carries no products", transaction.IdTransaction);
            return false;
        }

        var walletRequest = new WalletRequestModel
        {
            AffiliateId = ipn.item_number,
            AffiliateUserName = ipn.item_name,
            PurchaseFor = Constants.EmptyValue,
            Bank = Constants.CoinPayments,
            PaymentMethod = CoinPaymentsConstants.PaymentMethodId,
            ReceiptNumber = transaction.IdTransaction,
            BrandId = brandId,
            ProductsList = products
                .Select(p => new ProductsRequests { IdProduct = p.ProductId, Count = p.Quantity })
                .ToList()
        };

        var productType = await ResolveProductType(walletRequest, brandId);
        if (productType is null)
        {
            // Usually the inventory service being unreachable. Throwing rolls the whole IPN back
            // so a retry can credit, instead of leaving the row flagged without an invoice.
            throw new InvalidOperationException(
                $"Could not resolve the product type for CoinPayments transaction {transaction.IdTransaction}.");
        }

        // ExecuteMembershipPayment already credits the referral bonus to the parent through
        // IMembershipBonusService, so the old GrantWelcomeBonus is deliberately not ported —
        // running both would pay the bonus twice.
        var succeeded = productType == ProductType.Membership
            ? await _paymentStrategy.ExecuteMembershipPayment(walletRequest, Constants.CoinPayments)
            : await _paymentStrategy.ExecuteProductPayment(
                walletRequest, ToCoinPaymentType(productType.Value), Constants.CoinPayments);

        if (!succeeded)
        {
            throw new InvalidOperationException(
                $"Crediting the CoinPayments purchase for transaction {transaction.IdTransaction} failed.");
        }

        transaction.Acredited = true;

        _logger.LogInformation(
            "CoinPayments purchase credited as {ProductType} for affiliate {AffiliateId}",
            productType, transaction.AffiliateId);

        return true;
    }

    private List<ProductRequest> ReadProducts(Transaction transaction)
    {
        if (string.IsNullOrWhiteSpace(transaction.Products))
            return [];

        try
        {
            return transaction.Products.ToJsonObject<List<ProductRequest>>() ?? [];
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex, "Could not read the product list of CoinPayments txn {TxnId}", transaction.IdTransaction);
            return [];
        }
    }

    private async Task<ProductType?> ResolveProductType(WalletRequestModel request, long brandId)
    {
        var validation = await _productValidator.ValidateAndGetProducts(request.ProductsList, brandId);
        if (!validation.IsSuccess || validation.Products is null || validation.Products.Count == 0)
            return null;

        return validation.Products.First().PaymentGroup switch
        {
            1 => ProductType.Membership,
            2 or 7 or 8 => ProductType.EcoPool,
            11 => ProductType.RecyCoin,
            12 => ProductType.HouseCoinPlan,
            13 => ProductType.ExitoJuntosPlan,
            _ => ProductType.Course
        };
    }

    private static CoinPaymentType ToCoinPaymentType(ProductType productType) => productType switch
    {
        ProductType.EcoPool => CoinPaymentType.EcoPool,
        ProductType.RecyCoin => CoinPaymentType.RecyCoin,
        ProductType.HouseCoinPlan => CoinPaymentType.HouseCoin,
        ProductType.ExitoJuntosPlan => CoinPaymentType.ExitoJuntos,
        _ => CoinPaymentType.Course
    };
}
