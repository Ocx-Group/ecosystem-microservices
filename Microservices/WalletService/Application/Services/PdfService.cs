using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Services;
using Fluid;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Ecosystem.WalletService.Application.Services;

public class PdfService : IPdfService
{
    private readonly IConfigurationServicePdfAdapter _pdfAdapter;
    private readonly IBrandConfigurationProvider _brandConfigProvider;
    private readonly IBrowserProvider _browserProvider;
    private readonly ILogger<PdfService> _logger;

    private static readonly FluidParser Parser = new();

    public PdfService(
        IConfigurationServicePdfAdapter pdfAdapter,
        IBrandConfigurationProvider brandConfigProvider,
        IBrowserProvider browserProvider,
        ILogger<PdfService> logger)
    {
        _pdfAdapter = pdfAdapter;
        _brandConfigProvider = brandConfigProvider;
        _browserProvider = browserProvider;
        _logger = logger;
    }

    public async Task<byte[]> GenerateFromTemplate(string templateKey, long brandId, object data)
    {
        var template = await _pdfAdapter.GetTemplateAsync(brandId, templateKey);
        if (template is null)
            throw new InvalidOperationException($"PDF template '{templateKey}' not found for brand {brandId}");

        var brandConfig = await _brandConfigProvider.GetByBrandIdAsync(brandId);
        if (brandConfig is null)
            throw new InvalidOperationException($"Brand configuration not found for brand {brandId}");

        var html = RenderTemplate(template.HtmlContent, brandConfig, data);
        return await ConvertHtmlToPdf(html);
    }

    public async Task<byte[]> RegenerateInvoice(UserInfoResponse user, Invoice invoice)
    {
        var brandId = invoice.BrandId;

        var invoiceData = new
        {
            ReceiptNumber = invoice.InvoiceNumber.ToString(),
            Date = invoice.CreatedAt,
            Total = invoice.TotalInvoice ?? 0m,
            Subtotal = invoice.TotalInvoice ?? 0m,
            TaxTotal = 0m,
            Items = new List<object>()
        };

        var customerData = new
        {
            Name = user.Name ?? string.Empty,
            LastName = user.LastName ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Email = user.Email ?? string.Empty,
            Phone = user.Phone ?? string.Empty,
            Country = user.Country?.Name ?? string.Empty,
            City = user.City ?? string.Empty
        };

        return await GenerateFromTemplate("invoice", brandId, new { invoice = invoiceData, customer = customerData });
    }

    private static string RenderTemplate(string htmlTemplate, BrandConfigurationDto brandConfig, object data)
    {
        if (!Parser.TryParse(htmlTemplate, out var template, out var error))
            throw new InvalidOperationException($"Failed to parse template: {error}");

        var options = new TemplateOptions();
        options.MemberAccessStrategy.Register<BrandConfigurationDto>();
        options.MemberAccessStrategy.MemberNameStrategy = MemberNameStrategies.CamelCase;

        var context = new TemplateContext(data, options);
        context.SetValue("brand", brandConfig);

        return template.Render(context);
    }

    private async Task<byte[]> ConvertHtmlToPdf(string html)
    {
        var browser = await _browserProvider.GetBrowserAsync();
        await using var page = await browser.NewPageAsync();

        await page.SetContentAsync(html, new NavigationOptions { WaitUntil = [WaitUntilNavigation.Networkidle0] });

        var pdfData = await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.Letter,
            PrintBackground = true,
            MarginOptions = new MarginOptions
            {
                Top = "10mm",
                Bottom = "10mm",
                Left = "10mm",
                Right = "10mm"
            }
        });

        return pdfData;
    }
}
