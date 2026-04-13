using System.Net.Http.Json;
using Ecosystem.NotificationService.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Ecosystem.NotificationService.Application.Adapters;

/// <summary>
/// Fetches PDF HTML templates from ConfigurationService via HTTP.
/// TODO: Replace with gRPC or RabbitMQ when ConfigurationService exposes PDF template endpoints.
/// </summary>
public class ConfigurationServicePdfAdapter : IPdfTemplateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ConfigurationServicePdfAdapter> _logger;

    public ConfigurationServicePdfAdapter(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        ILogger<ConfigurationServicePdfAdapter> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ConfigurationService");
        var baseUrl = configuration["Services:ConfigurationService:BaseUrl"]
            ?? "http://localhost:5003";
        _httpClient.BaseAddress = new Uri(baseUrl);
        _logger = logger;
    }

    public async Task<PdfTemplate?> GetTemplateAsync(long brandId, string templateKey)
    {
        try
        {
            var response = await _httpClient.GetAsync(
                $"/api/v1/PdfTemplate/{brandId}/{templateKey}");

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning(
                    "PDF template '{TemplateKey}' not found for brand {BrandId}: {StatusCode}",
                    templateKey, brandId, response.StatusCode);
                return null;
            }

            var dto = await response.Content.ReadFromJsonAsync<PdfTemplateResponse>();
            return dto is null ? null : new PdfTemplate(dto.HtmlContent, dto.CssContent, dto.Version);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching PDF template '{TemplateKey}' for brand {BrandId}",
                templateKey, brandId);
            return null;
        }
    }

    private record PdfTemplateResponse(string HtmlContent, string? CssContent, int Version);
}
