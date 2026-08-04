using System.Text.Json;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPay;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Requests.CoinPayRequest;
using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;
using ProductsRequests = Ecosystem.WalletService.Domain.Requests.WalletRequest.ProductsRequests;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class ProcessCoinPayWebhookHandler : IRequestHandler<ProcessCoinPayWebhookCommand, bool>
{
    /// <summary>Payment method id recorded on the wallet request for CoinPay purchases.</summary>
    private const int CoinPayPaymentMethodId = 4;

    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAccountServiceAdapter _accountAdapter;
    private readonly IProductValidationService _productValidator;
    private readonly IExternalPaymentStrategy _paymentStrategy;
    private readonly ICacheService _cacheService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ApplicationConfiguration _appSettings;
    private readonly ILogger<ProcessCoinPayWebhookHandler> _logger;

    public ProcessCoinPayWebhookHandler(
        ICoinPayAdapter coinPayAdapter,
        ITransactionRepository transactionRepository,
        IInvoiceRepository invoiceRepository,
        IAccountServiceAdapter accountAdapter,
        IProductValidationService productValidator,
        IExternalPaymentStrategy paymentStrategy,
        ICacheService cacheService,
        IUnitOfWork unitOfWork,
        IOptions<ApplicationConfiguration> appSettings,
        ILogger<ProcessCoinPayWebhookHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _transactionRepository = transactionRepository;
        _invoiceRepository = invoiceRepository;
        _accountAdapter = accountAdapter;
        _productValidator = productValidator;
        _paymentStrategy = paymentStrategy;
        _cacheService = cacheService;
        _unitOfWork = unitOfWork;
        _appSettings = appSettings.Value;
        _logger = logger;
    }

    public async Task<bool> Handle(ProcessCoinPayWebhookCommand request, CancellationToken cancellationToken)
    {
        var notification = request.Request;

        if (!await IsSignatureAccepted(notification, request.Signature))
        {
            _logger.LogWarning(
                "Rejected CoinPay webhook for transaction {IdTransaction}: invalid signature",
                notification.IdTransaction);
            return false;
        }

        var reference = notification.UserChannel?.IdExternalIdentification;
        if (string.IsNullOrEmpty(reference))
        {
            _logger.LogWarning("CoinPay webhook has no channel external identification; nothing to match on");
            return false;
        }

        var transaction = await _transactionRepository.GetTransactionByReference(reference);
        if (transaction is null)
        {
            _logger.LogWarning("No transaction found for CoinPay reference {Reference}", reference);
            return false;
        }

        // The webhook is anonymous, so the brand comes from the stored transaction
        // rather than from the (unresolved) tenant context.
        var brandId = transaction.BrandId;

        if (notification.Amount < transaction.Amount)
        {
            _logger.LogWarning(
                "CoinPay reported {Received} for reference {Reference} but {Expected} was expected",
                notification.Amount, reference, transaction.Amount);
            return false;
        }

        if (transaction.Acredited || transaction.Status == Constants.CompletedStatusCode)
        {
            _logger.LogInformation("CoinPay reference {Reference} was already credited", reference);
            return false;
        }

        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var credited = false;

            switch (notification.TransactionStatus.Id)
            {
                case Constants.CoinPayPendingStatusCode:
                    transaction.Status = Constants.RegisteredStatusCode;
                    transaction.Acredited = false;
                    transaction.IdTransaction = notification.IdTransaction.ToString();
                    break;

                case Constants.CoinPayExpiredStatusCode:
                    transaction.Status = Constants.ExpiredStatusCode;
                    transaction.Acredited = false;
                    break;

                case Constants.CoinPaySuccessStatusCode:
                    transaction.Status = Constants.CompletedStatusCode;
                    transaction.AmountReceived = notification.Amount;
                    transaction.Acredited = true;
                    transaction.IdTransaction = notification.IdTransaction.ToString();
                    credited = true;
                    break;

                default:
                    // Status left untouched, as in the original: an unknown code is logged
                    // and the row is saved unchanged.
                    _logger.LogWarning(
                        "Unknown CoinPay transaction status {StatusId} for reference {Reference}",
                        notification.TransactionStatus.Id, reference);
                    break;
            }

            transaction.UpdatedAt = DateTime.UtcNow;
            await _transactionRepository.UpdateTransactionAsync(transaction);

            // Credit inside the same transaction so a failure here cannot leave the row
            // flagged as credited without the matching invoice.
            if (credited)
                await CreditPurchase(transaction, brandId);

            await _unitOfWork.CommitAsync();

            if (credited)
                await _cacheService.InvalidateBalanceAsync(transaction.AffiliateId);

            _logger.LogInformation(
                "CoinPay webhook processed for reference {Reference} with status {Status}",
                reference, transaction.Status);

            return true;
        }
        catch (Exception ex)
        {
            await _unitOfWork.RollbackAsync();
            _logger.LogError(ex, "Failed to process the CoinPay webhook for reference {Reference}", reference);
            return false;
        }
    }

    private async Task CreditPurchase(Transaction transaction, long brandId)
    {
        var userInfo = await _accountAdapter.GetUserInfo(transaction.AffiliateId, brandId);
        if (userInfo?.UserName is null)
        {
            // Usually the account service being unreachable — roll back so the retry can credit.
            throw new InvalidOperationException(
                $"No account found for affiliate {transaction.AffiliateId}.");
        }

        if (await _invoiceRepository.InvoiceExistsByReceiptNumber(transaction.IdTransaction, brandId))
        {
            _logger.LogInformation(
                "Invoice already exists for CoinPay transaction {IdTransaction}", transaction.IdTransaction);
            return;
        }

        var products = JsonSerializer.Deserialize<List<ProductRequest>>(transaction.Products);
        if (products is null || products.Count == 0)
        {
            _logger.LogWarning(
                "CoinPay transaction {IdTransaction} carries no products", transaction.IdTransaction);
            return;
        }

        var walletRequest = new WalletRequestModel
        {
            AffiliateId = transaction.AffiliateId,
            AffiliateUserName = userInfo.UserName,
            PurchaseFor = Constants.EmptyValue,
            Bank = transaction.Reference,
            PaymentMethod = CoinPayPaymentMethodId,
            ReceiptNumber = transaction.IdTransaction,
            BrandId = brandId,
            ProductsList = products
                .Select(p => new ProductsRequests { IdProduct = p.ProductId, Count = p.Quantity })
                .ToList()
        };

        var productType = await ResolveProductType(walletRequest, brandId);
        if (productType is null)
        {
            // Same reasoning: an inventory lookup failure must not leave the row credited.
            throw new InvalidOperationException(
                $"Could not resolve the product type for CoinPay transaction {transaction.IdTransaction}.");
        }

        var succeeded = productType == ProductType.Membership
            ? await _paymentStrategy.ExecuteMembershipPayment(walletRequest, Constants.CoinPay)
            : await _paymentStrategy.ExecuteProductPayment(
                walletRequest, ToCoinPaymentType(productType.Value), Constants.CoinPay);

        if (!succeeded)
        {
            throw new InvalidOperationException(
                $"Crediting the CoinPay purchase for transaction {transaction.IdTransaction} failed.");
        }

        _logger.LogInformation(
            "CoinPay purchase credited as {ProductType} for affiliate {AffiliateId}",
            productType, transaction.AffiliateId);
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

    /// <summary>
    /// Verifies the provider's HMAC when signature checking is switched on. It stays off
    /// until CoinPay confirms the header name and dynamic key, since a wrong guess would
    /// reject every legitimate notification.
    /// </summary>
    private async Task<bool> IsSignatureAccepted(WebhookNotificationRequest notification, string? signature)
    {
        if (_appSettings.CoinPay?.RequireWebhookSignature != true)
        {
            _logger.LogDebug("CoinPay webhook signature verification is disabled");
            return true;
        }

        return await _coinPayAdapter.VerifyTransactionSignature(new SignatureParamsRequest
        {
            IdUser = notification.IdUser,
            IdTransaction = notification.IdTransaction,
            DynamicKey = _appSettings.CoinPay.DynamicKey ?? string.Empty,
            IncomingSignature = signature
        });
    }
}
