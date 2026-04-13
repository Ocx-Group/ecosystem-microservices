using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Services;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Strategies;

public class CoinPaymentsPaymentStrategy : ICoinPaymentsPaymentStrategy
{
    private readonly IProductValidationService _productValidator;
    private readonly IPaymentCalculator _calculator;
    private readonly IInvoiceDetailFactory _invoiceFactory;
    private readonly IDebitTransactionBuilder _debitBuilder;
    private readonly IPaymentNotificationService _notifications;
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IAccountServiceAdapter _accountAdapter;
    private readonly IMembershipBonusService _membershipBonus;
    private readonly IBrandConfigurationProvider _brandConfigProvider;

    public CoinPaymentsPaymentStrategy(
        IProductValidationService productValidator,
        IPaymentCalculator calculator,
        IInvoiceDetailFactory invoiceFactory,
        IDebitTransactionBuilder debitBuilder,
        IPaymentNotificationService notifications,
        IInvoiceRepository invoiceRepository,
        IWalletRepository walletRepository,
        IAccountServiceAdapter accountAdapter,
        IMembershipBonusService membershipBonus,
        IBrandConfigurationProvider brandConfigProvider)
    {
        _productValidator = productValidator;
        _calculator = calculator;
        _invoiceFactory = invoiceFactory;
        _debitBuilder = debitBuilder;
        _notifications = notifications;
        _invoiceRepository = invoiceRepository;
        _walletRepository = walletRepository;
        _accountAdapter = accountAdapter;
        _membershipBonus = membershipBonus;
        _brandConfigProvider = brandConfigProvider;
    }

    public async Task<bool> ExecuteProductPayment(WalletRequestModel request, CoinPaymentType paymentType)
    {
        var validation = await _productValidator.ValidateAndGetProducts(request.ProductsList, request.BrandId);
        if (!validation.IsSuccess) return false;

        var calc = _calculator.Calculate(validation.Products!, request.ProductsList);
        if (calc.Debit == Constants.EmptyValue) return false;

        var invoiceDetails = _invoiceFactory.CreateDetails(validation.Products!, request.ProductsList, request.BrandId);

        var reason = ResolveReason(request, paymentType);

        var debitRequest = await _debitBuilder.Reset()
            .ForAffiliate(request.AffiliateId, request.AffiliateUserName)
            .WithConcept(Constants.EcoPoolProductCategory, nameof(WalletConceptType.purchasing_pool))
            .WithAmounts(calc)
            .WithPaymentMethod(Constants.CoinPayments)
            .WithInvoiceDetails(invoiceDetails)
            .WithReceiptInfo(Constants.CoinPayments, request.ReceiptNumber, true)
            .WithSecretKey(request.SecretKey)
            .WithReason(reason)
            .BuildAsync(request.BrandId);

        var result = await _invoiceRepository.HandleDebitTransaction(debitRequest);
        if (result == null) return false;

        await ExecutePostPaymentActions(request, debitRequest, result, invoiceDetails, paymentType);

        return true;
    }

    public async Task<bool> ExecuteMembershipPayment(WalletRequestModel request)
    {
        var userInfo = await _accountAdapter.GetUserInfo(request.AffiliateId, request.BrandId);
        if (userInfo == null) return false;

        var validation = await _productValidator.ValidateAndGetProducts(request.ProductsList, request.BrandId);
        if (!validation.IsSuccess) return false;

        var calc = _calculator.Calculate(validation.Products!, request.ProductsList);
        if (calc.Debit == Constants.EmptyValue) return false;

        var invoiceDetails = _invoiceFactory.CreateDetails(validation.Products!, request.ProductsList, request.BrandId);

        var debitRequest = await _debitBuilder.Reset()
            .ForAffiliate(request.AffiliateId, request.AffiliateUserName)
            .WithConcept(Constants.Membership, nameof(WalletConceptType.purchasing_pool))
            .WithAmounts(calc)
            .WithPaymentMethod(Constants.CoinPayments)
            .WithInvoiceDetails(invoiceDetails)
            .WithReceiptInfo(Constants.CoinPayments, request.ReceiptNumber, true)
            .WithSecretKey(request.SecretKey)
            .BuildAsync(request.BrandId);

        var result = await _walletRepository.HandleMembershipTransaction(debitRequest);
        if (result == null) return false;

        // TODO: Add UpdateActivationDate when IAccountServiceAdapter supports it
        await _membershipBonus.CreditBonusToParentAsync(userInfo, request);
        await _notifications.SendMembershipConfirmation(userInfo, result, request);

        if (userInfo.Father > 0)
        {
            var parentInfo = await _accountAdapter.GetUserInfo(userInfo.Father, request.BrandId);
            if (parentInfo != null)
                await _notifications.SendBonusNotification(parentInfo, request.AffiliateUserName, request.BrandId);
        }

        return true;
    }

    private async Task ExecutePostPaymentActions(
        WalletRequestModel request,
        DebitTransactionRequest debitRequest,
        Domain.CustomModels.InvoicesSpResponse result,
        List<InvoiceDetailsTransactionRequest> invoiceDetails,
        CoinPaymentType paymentType)
    {
        var userInfo = await _accountAdapter.GetUserInfo(request.AffiliateId, request.BrandId);

        if (paymentType == CoinPaymentType.HouseCoin)
        {
            var brandConfig = await _brandConfigProvider.GetByBrandIdAsync(request.BrandId);

            if (brandConfig is { CommissionEnabled: true, CommissionLevels.Length: > 0 })
            {
                await _walletRepository.DistributeCommissionsPerPurchaseAsync(new DistributeCommissionsRequest
                {
                    AffiliateId = request.AffiliateId,
                    InvoiceAmount = debitRequest.Debit,
                    BrandId = request.BrandId,
                    AdminUserName = brandConfig.AdminUserName,
                    LevelPercentages = brandConfig.CommissionLevels,
                });
            }
        }

        if (userInfo != null)
            await _notifications.SendPaymentConfirmation(userInfo, result, request, invoiceDetails);
    }

    private static string? ResolveReason(WalletRequestModel request, CoinPaymentType paymentType) => paymentType switch
    {
        CoinPaymentType.RecyCoin or CoinPaymentType.HouseCoin => request.Bank,
        _ => string.Empty
    };
}
