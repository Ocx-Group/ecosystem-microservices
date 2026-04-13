using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Grpc.Configuration;
using Grpc.Core;

namespace Ecosystem.ConfigurationService.Api.GrpcServices;

public class ConfigurationGrpcService : ConfigurationGrpc.ConfigurationGrpcBase
{
    private readonly IPdfTemplateRepository _pdfTemplateRepository;
    private readonly ILogger<ConfigurationGrpcService> _logger;

    public ConfigurationGrpcService(
        IPdfTemplateRepository pdfTemplateRepository,
        ILogger<ConfigurationGrpcService> logger)
    {
        _pdfTemplateRepository = pdfTemplateRepository;
        _logger = logger;
    }

    public override async Task<GetPdfTemplateResponse> GetPdfTemplate(
        GetPdfTemplateRequest request, ServerCallContext context)
    {
        try
        {
            var template = await _pdfTemplateRepository.GetByBrandAndKeyAsync(
                request.BrandId, request.TemplateKey);

            if (template is null || !template.IsActive)
            {
                return new GetPdfTemplateResponse
                {
                    Success = false,
                    Message = $"Template '{request.TemplateKey}' not found for brand {request.BrandId}"
                };
            }

            return new GetPdfTemplateResponse
            {
                Success = true,
                HtmlContent = template.HtmlContent,
                CssContent = template.CssContent ?? string.Empty,
                Version = template.Version
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching PDF template '{TemplateKey}' for brand {BrandId}",
                request.TemplateKey, request.BrandId);

            return new GetPdfTemplateResponse
            {
                Success = false,
                Message = "Internal error retrieving PDF template"
            };
        }
    }
}
