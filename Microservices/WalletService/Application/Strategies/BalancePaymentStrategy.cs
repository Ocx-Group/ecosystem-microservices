using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Events;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Enums;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Services;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Strategies;

public class BalancePaymentStrategy : IBalancePaymentStrategy
{
    private readonly IProductValidationService _productValidator;
    private readonly IPaymentCalculator _calculator;
    private readonly IInvoiceDetailFactory _invoiceFactory;
    private readonly IDebitTransactionBuilder _debitBuilder;
    private readonly IBalanceValidationService _balanceValidator;
    private readonly IPaymentNotificationService _notifications;
    private readonly IWalletRepository _walletRepository;
    private readonly IAccountServiceAdapter _accountAdapter;
    private readonly IMembershipBonusService _membershipBonus;
    private readonly IEventBus _eventBus;

    public BalancePaymentStrategy(
        IProductValidationService productValidator,
        IPaymentCalculator calculator,
        IInvoiceDetailFactory invoiceFactory,
        IDebitTransactionBuilder debitBuilder,
        IBalanceValidationService balanceValidator,
        IPaymentNotificationService notifications,
        IWalletRepository walletRepository,
        IAccountServiceAdapter accountAdapter,
        IMembershipBonusService membershipBonus,
        IEventBus eventBus)
    {
        _productValidator = productValidator;
        _calculator = calculator;
        _invoiceFactory = invoiceFactory;
        _debitBuilder = debitBuilder;
        _balanceValidator = balanceValidator;
        _notifications = notifications;
        _walletRepository = walletRepository;
        _accountAdapter = accountAdapter;
        _membershipBonus = membershipBonus;
        _eventBus = eventBus;
    }

    public async Task<bool> ExecuteProductPayment(WalletRequestModel request)
    {
        var validation = await _productValidator.ValidateAndGetProducts(request.ProductsList, request.BrandId);
        if (!validation.IsSuccess) return false;

        var calc = _calculator.Calculate(validation.Products!, request.ProductsList);
        if (calc.Debit == Constants.EmptyValue) return false;

        var balance = await _balanceValidator.ValidateBalance(request.AffiliateId, request.BrandId, calc.Debit);
        if (!balance.IsSuccess) return false;

        var invoiceDetails = _invoiceFactory.CreateDetails(validation.Products!, request.ProductsList, request.BrandId);

        var debitRequest = await _debitBuilder.Reset()
            .ForAffiliate(request.AffiliateId, request.AffiliateUserName)
            .WithConcept(Constants.EcoPoolProductCategory, nameof(WalletConceptType.purchasing_pool))
            .WithAmounts(calc)
            .WithPaymentMethod(Constants.WalletBalance)
            .WithInvoiceDetails(invoiceDetails)
            .WithCommissionCalculation(request.IncludeInCommissionCalculation)
            .BuildAsync(request.BrandId);

        var result = await _walletRepository.DebitEcoPoolTransactionSp(debitRequest);
        if (result == null) return false;

        var userInfo = await _accountAdapter.GetUserInfo(request.AffiliateId, request.BrandId);
        if (userInfo != null)
            await _notifications.SendPaymentConfirmation(userInfo, result, request, invoiceDetails);

        return true;
    }

    public async Task<bool> ExecuteAdminPayment(WalletRequestModel request, decimal? customPrice = null)
    {
        var validation = await _productValidator.ValidateAndGetProducts(request.ProductsList, request.BrandId);
        if (!validation.IsSuccess) return false;

        if (customPrice.HasValue)
        {
            if (customPrice.Value == Constants.EmptyValue) return false;
            ApplyCustomPricing(validation.Products!, customPrice.Value);
        }

        var calc = _calculator.Calculate(validation.Products!, request.ProductsList);
        if (calc.Debit == Constants.EmptyValue) return false;

        var invoiceDetails = _invoiceFactory.CreateDetails(validation.Products!, request.ProductsList, request.BrandId);

        var debitRequest = await _debitBuilder.Reset()
            .ForAffiliate(request.AffiliateId, request.AffiliateUserName)
            .WithConcept(Constants.EcoPoolProductCategoryForAdmin, nameof(WalletConceptType.purchasing_pool))
            .WithAmounts(calc)
            .WithPaymentMethod(Constants.AdminPayment)
            .WithInvoiceDetails(invoiceDetails)
            .WithReceiptInfo(null, null, true)
            .WithCommissionCalculation(request.IncludeInCommissionCalculation)
            .BuildAsync(request.BrandId);

        var result = await _walletRepository.AdminDebitTransaction(debitRequest);
        if (result == null) return false;

        if (customPrice.HasValue)
        {
            var userInfo = await _accountAdapter.GetUserInfo(request.AffiliateId, request.BrandId);
            if (userInfo != null)
                await _notifications.SendPaymentConfirmation(userInfo, result, request, invoiceDetails);
        }

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

        var balance = await _balanceValidator.ValidateBalance(request.AffiliateId, request.BrandId, calc.Debit);
        if (!balance.IsSuccess) return false;

        var invoiceDetails = _invoiceFactory.CreateDetails(validation.Products!, request.ProductsList, request.BrandId);

        var debitRequest = await _debitBuilder.Reset()
            .ForAffiliate(request.AffiliateId, request.AffiliateUserName)
            .WithConcept(Constants.Membership, nameof(WalletConceptType.purchasing_pool))
            .WithAmounts(calc)
            .WithPaymentMethod(Constants.WalletBalance)
            .WithInvoiceDetails(invoiceDetails)
            .WithReceiptInfo(null, null, true)
            .BuildAsync(request.BrandId);

        var result = await _walletRepository.MembershipDebitTransaction(debitRequest);
        if (result == null) return false;

        await _eventBus.Publish(new UpdateActivationDateEvent(request.AffiliateId, request.BrandId));
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

    private static void ApplyCustomPricing(List<Domain.DTOs.ProductWalletDto.ProductWalletDto> products, decimal customPrice)
    {
        foreach (var product in products)
        {
            product.SalePrice = customPrice;
            product.BaseAmount = customPrice;
            product.Name = "CustomEcoPool";
        }
    }
}
