using Ecosystem.Grpc.Configuration;
using Ecosystem.NotificationService.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Adapters;

public class GrpcConfigurationServiceAdapter : IPdfTemplateProvider
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
}
