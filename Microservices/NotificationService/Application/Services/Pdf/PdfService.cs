using Ecosystem.NotificationService.Domain.Interfaces;
using Fluid;
using Microsoft.Extensions.Logging;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace Ecosystem.NotificationService.Application.Services.Pdf;

public class PdfService : IPdfService
{
    private readonly IPdfTemplateProvider _templateProvider;
    private readonly IBrandConfigurationRepository _brandRepository;
    private readonly IBrowserProvider _browserProvider;
    private readonly ILogger<PdfService> _logger;

    private static readonly FluidParser Parser = new();

    public PdfService(
        IPdfTemplateProvider templateProvider,
        IBrandConfigurationRepository brandRepository,
        IBrowserProvider browserProvider,
        ILogger<PdfService> logger)
    {
        _templateProvider = templateProvider;
        _brandRepository = brandRepository;
        _browserProvider = browserProvider;
        _logger = logger;
    }

    public async Task<byte[]> GenerateFromTemplateAsync(string templateKey, long brandId, object data)
    {
        var template = await _templateProvider.GetTemplateAsync(brandId, templateKey);
        if (template is null)
        {
            _logger.LogWarning("PDF template '{TemplateKey}' not found for brand {BrandId}", templateKey, brandId);
            return [];
        }

        var brandConfig = await _brandRepository.GetByBrandIdAsync(brandId);
        if (brandConfig is null)
        {
            _logger.LogWarning("Brand configuration not found for brand {BrandId}", brandId);
            return [];
        }

        var html = RenderTemplate(template.HtmlContent, brandConfig, data);
        return await ConvertHtmlToPdfAsync(html);
    }

    private static string RenderTemplate(string htmlTemplate, Domain.Models.BrandConfiguration brandConfig, object data)
    {
        if (!Parser.TryParse(htmlTemplate, out var template, out var error))
            throw new InvalidOperationException($"Failed to parse template: {error}");

        var options = new TemplateOptions();
        options.MemberAccessStrategy.Register<Domain.Models.BrandConfiguration>();
        options.MemberAccessStrategy.MemberNameStrategy = MemberNameStrategies.CamelCase;

        var context = new TemplateContext(data, options);
        context.SetValue("brand", brandConfig);

        return template.Render(context);
    }

    private async Task<byte[]> ConvertHtmlToPdfAsync(string html)
    {
        var browser = await _browserProvider.GetBrowserAsync();
        await using var page = await browser.NewPageAsync();

        await page.SetContentAsync(html, new NavigationOptions { WaitUntil = [WaitUntilNavigation.Networkidle0] });

        return await page.PdfDataAsync(new PdfOptions
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
    }
}
