using Ecosystem.Domain.Core.Bus;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.Domain.Core.Events;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Services;
using Microsoft.Extensions.Logging;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Services;

public class PaymentNotificationService : IPaymentNotificationService
{
    private readonly IEventBus _eventBus;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PaymentNotificationService> _logger;

    public PaymentNotificationService(
        IEventBus eventBus,
        ICacheService cacheService,
        ILogger<PaymentNotificationService> logger)
    {
        _eventBus = eventBus;
        _cacheService = cacheService;
        _logger = logger;
    }

    public async Task SendPaymentConfirmation(
        UserInfoResponse userInfo,
        InvoicesSpResponse transactionResponse,
        WalletRequestModel request,
        List<InvoiceDetailsTransactionRequest> invoiceDetails)
    {
        await InvalidateBalanceCache(request.AffiliateId);

        try
        {
            var items = invoiceDetails.Select(d => new InvoiceItemEventData(
                d.ProductName,
                d.ProductQuantity,
                d.ProductPrice,
                d.ProductDiscount,
                (d.ProductPrice * d.ProductQuantity) - d.ProductDiscount
            )).ToList();

            var invoiceData = new InvoiceEventData(
                ReceiptNumber: transactionResponse.InvoiceNumber.ToString(),
                Date: transactionResponse.Date ?? DateTime.UtcNow,
                Total: transactionResponse.TotalInvoice ?? 0m,
                Subtotal: items.Sum(i => i.Price * i.Quantity),
                TaxTotal: (transactionResponse.TotalInvoice ?? 0m) - items.Sum(i => i.Total));

            var customerData = BuildCustomerData(userInfo);

            await _eventBus.Publish(new SendPaymentConfirmationEvent(
                request.BrandId,
                userInfo.Email ?? string.Empty,
                userInfo.Name ?? string.Empty,
                invoiceData,
                customerData,
                items));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing payment confirmation for affiliate {AffiliateId}", request.AffiliateId);
        }
    }

    public async Task SendMembershipConfirmation(
        UserInfoResponse userInfo,
        InvoicesSpResponse transactionResponse,
        WalletRequestModel request)
    {
        await InvalidateBalanceCache(request.AffiliateId);

        try
        {
            var invoiceData = new InvoiceEventData(
                ReceiptNumber: transactionResponse.InvoiceNumber.ToString(),
                Date: transactionResponse.Date ?? DateTime.UtcNow,
                Total: transactionResponse.TotalInvoice ?? 0m,
                Subtotal: transactionResponse.TotalInvoice ?? 0m,
                TaxTotal: 0m);

            var customerData = BuildCustomerData(userInfo);

            await _eventBus.Publish(new SendMembershipConfirmationEvent(
                request.BrandId,
                userInfo.Email ?? string.Empty,
                userInfo.Name ?? string.Empty,
                invoiceData,
                customerData));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing membership confirmation for affiliate {AffiliateId}", request.AffiliateId);
        }
    }

    public async Task SendBonusNotification(
        UserInfoResponse bonusWinner,
        string affiliateUserName,
        long brandId)
    {
        try
        {
            await InvalidateBalanceCache(bonusWinner.Id);

            await _eventBus.Publish(new SendBonusNotificationEvent(
                brandId,
                bonusWinner.Email ?? string.Empty,
                bonusWinner.Name ?? string.Empty,
                affiliateUserName));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing bonus notification for affiliate {AffiliateId}", bonusWinner.Id);
        }
    }

    private async Task InvalidateBalanceCache(int affiliateId)
    {
        var key = string.Format(Domain.Constants.CacheKeys.BalanceInformationModel2, affiliateId);
        await _cacheService.Delete(key);
    }

    private static CustomerEventData BuildCustomerData(UserInfoResponse user) => new(
        Name: user.Name ?? string.Empty,
        LastName: user.LastName ?? string.Empty,
        UserName: user.UserName ?? string.Empty,
        Email: user.Email ?? string.Empty,
        Phone: user.Phone ?? string.Empty,
        Country: user.Country?.Name ?? string.Empty,
        City: user.City ?? string.Empty);
}
