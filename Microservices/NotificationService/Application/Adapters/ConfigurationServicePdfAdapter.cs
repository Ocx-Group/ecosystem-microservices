using Ecosystem.Grpc.Configuration;
using Ecosystem.NotificationService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Adapters;

public class GrpcConfigurationServiceAdapter : IPdfTemplateProvider, IBrandConfigurationReader
{
    private readonly ConfigurationGrpc.ConfigurationGrpcClient _client;
    private readonly ILogger<GrpcConfigurationServiceAdapter> _logger;

    public GrpcConfigurationServiceAdapter(
        ConfigurationGrpc.ConfigurationGrpcClient client,
        ILogger<GrpcConfigurationServiceAdapter> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<PdfTemplate?> GetTemplateAsync(long brandId, string templateKey)
    {
        try
        {
            var response = await _client.GetPdfTemplateAsync(new GetPdfTemplateRequest
            {
                BrandId = brandId,
                TemplateKey = templateKey
            });

            if (!response.Success)
            {
                _logger.LogWarning("PDF template '{TemplateKey}' not found for brand {BrandId}: {Message}",
                    templateKey, brandId, response.Message);
                return null;
            }

            return new PdfTemplate(
                response.HtmlContent,
                string.IsNullOrEmpty(response.CssContent) ? null : response.CssContent,
                response.Version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching PDF template '{TemplateKey}' for brand {BrandId}",
                templateKey, brandId);
            return null;
        }
    }

    public async Task<NotificationBrandConfiguration?> GetByBrandIdAsync(
        long brandId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetBrandConfigurationAsync(
                new GetBrandConfigurationRequest { BrandId = brandId },
                deadline: DateTime.UtcNow.AddSeconds(10),
                cancellationToken: cancellationToken);

            if (!response.Success || response.Configuration is null)
            {
                _logger.LogWarning(
                    "Brand configuration not found for brand {BrandId}: {Message}",
                    brandId,
                    response.Message);
                return null;
            }

            var source = response.Configuration;
            return new NotificationBrandConfiguration
            {
                BrandId = source.BrandId,
                Name = source.Name,
                SenderName = source.SenderName,
                SenderEmail = source.SenderEmail,
                ClientUrl = source.ClientUrl,
                CompanyName = source.CompanyName,
                SupportEmail = source.SupportEmail,
                CompanyIdentifier = source.HasCompanyIdentifier ? source.CompanyIdentifier : null,
                SupportPhone = source.HasSupportPhone ? source.SupportPhone : null,
                DocumentType = source.HasDocumentType ? source.DocumentType : null,
                LogoUrl = source.HasLogoUrl ? source.LogoUrl : null,
                PrimaryColor = source.PrimaryColor,
                SecondaryColor = source.SecondaryColor,
                BackgroundColor = source.BackgroundColor
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching brand configuration for brand {BrandId}", brandId);
            throw;
        }
    }
}
