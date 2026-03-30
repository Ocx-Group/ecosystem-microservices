using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.CustomModels;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Services;
using Microsoft.Extensions.Logging;
using WalletRequestModel = Ecosystem.WalletService.Domain.Requests.WalletRequest.WalletRequest;

namespace Ecosystem.WalletService.Application.Services;

public class PaymentNotificationService : IPaymentNotificationService
{
    private readonly IPdfService _pdfService;
    private readonly IBrandConfigurationProvider _brandConfigProvider;
    private readonly IEmailServiceAdapter _emailAdapter;
    private readonly ICacheService _cacheService;
    private readonly ILogger<PaymentNotificationService> _logger;

    public PaymentNotificationService(
        IPdfService pdfService,
        IBrandConfigurationProvider brandConfigProvider,
        IEmailServiceAdapter emailAdapter,
        ICacheService cacheService,
        ILogger<PaymentNotificationService> logger)
    {
        _pdfService = pdfService;
        _brandConfigProvider = brandConfigProvider;
        _emailAdapter = emailAdapter;
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
            var invoiceData = BuildInvoiceData(transactionResponse, invoiceDetails);
            var customerData = BuildCustomerData(userInfo);
            var templateData = new { invoice = invoiceData, customer = customerData };

            var pdfBytes = await _pdfService.GenerateFromTemplate("invoice", request.BrandId, templateData);

            var attachments = new Dictionary<string, byte[]>();
            if (pdfBytes.Length > 0)
                attachments["Factura.pdf"] = pdfBytes;

            if (attachments.Count > 0)
                await _emailAdapter.SendEmailPurchaseConfirm(userInfo, attachments, transactionResponse, request.BrandId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending payment confirmation for affiliate {AffiliateId}", request.AffiliateId);
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
            var invoiceData = new
            {
                ReceiptNumber = transactionResponse.InvoiceNumber.ToString(),
                Date = transactionResponse.Date ?? DateTime.UtcNow,
                Total = transactionResponse.TotalInvoice ?? 0m,
                Subtotal = transactionResponse.TotalInvoice ?? 0m,
                TaxTotal = 0m
            };
            var customerData = BuildCustomerData(userInfo);
            var templateData = new { invoice = invoiceData, customer = customerData };

            var pdfBytes = await _pdfService.GenerateFromTemplate("membership", request.BrandId, templateData);

            await _emailAdapter.SendEmailWelcome(userInfo, transactionResponse, request.BrandId);

            if (pdfBytes.Length > 0)
                await _emailAdapter.SendEmailMembershipConfirm(userInfo, pdfBytes, transactionResponse, request.BrandId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending membership confirmation for affiliate {AffiliateId}", request.AffiliateId);
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
            await _emailAdapter.SendBonusConfirmation(bonusWinner, affiliateUserName, brandId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending bonus notification for affiliate {AffiliateId}", bonusWinner.Id);
        }
    }

    private async Task InvalidateBalanceCache(int affiliateId)
    {
        var key = string.Format(Domain.Constants.CacheKeys.BalanceInformationModel2, affiliateId);
        await _cacheService.Delete(key);
    }

    private static object BuildInvoiceData(
        InvoicesSpResponse transaction,
        List<InvoiceDetailsTransactionRequest> details)
    {
        var items = details.Select(d => new
        {
            d.ProductName,
            Quantity = d.ProductQuantity,
            Price = d.ProductPrice,
            Discount = d.ProductDiscount,
            Total = (d.ProductPrice * d.ProductQuantity) - d.ProductDiscount
        }).ToList();

        return new
        {
            ReceiptNumber = transaction.InvoiceNumber.ToString(),
            Date = transaction.Date ?? DateTime.UtcNow,
            Total = transaction.TotalInvoice ?? 0m,
            Subtotal = items.Sum(i => i.Price * i.Quantity),
            TaxTotal = (transaction.TotalInvoice ?? 0m) - items.Sum(i => i.Total),
            Items = items
        };
    }

    private static object BuildCustomerData(UserInfoResponse user)
    {
        return new
        {
            Name = user.Name ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Phone = user.Phone ?? string.Empty,
            Country = user.Country?.Name ?? string.Empty,
            City = user.City ?? string.Empty
        };
    }
}
