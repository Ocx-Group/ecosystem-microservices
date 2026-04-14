using AutoMapper;
using Ecosystem.ConfigurationService.Application.Queries.MatrixConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Grpc.Configuration;
using Grpc.Core;
using MediatR;

namespace Ecosystem.ConfigurationService.Api.GrpcServices;

public class ConfigurationGrpcService : ConfigurationGrpc.ConfigurationGrpcBase
{
    private readonly IPdfTemplateRepository _pdfTemplateRepository;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<ConfigurationGrpcService> _logger;

    public ConfigurationGrpcService(
        IPdfTemplateRepository pdfTemplateRepository,
        IMediator mediator,
        IMapper mapper,
        ILogger<ConfigurationGrpcService> logger)
    {
        _pdfTemplateRepository = pdfTemplateRepository;
        _mediator = mediator;
        _mapper = mapper;
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

    public override async Task<GetMatrixConfigurationResponse> GetMatrixConfiguration(
        GetMatrixConfigurationRequest request, ServerCallContext context)
    {
        try
        {
            var config = await _mediator.Send(new GetMatrixConfigurationByTypeQuery(request.MatrixType));

            if (config is null)
                return new GetMatrixConfigurationResponse { Success = false, Message = "Configuration not found" };

            return new GetMatrixConfigurationResponse
            {
                Success = true,
                Configuration = _mapper.Map<MatrixConfigMessage>(config)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting matrix config for type {MatrixType}", request.MatrixType);
            return new GetMatrixConfigurationResponse { Success = false, Message = "Internal error" };
        }
    }

    public override async Task<GetAllMatrixConfigurationsResponse> GetAllMatrixConfigurations(
        GetAllMatrixConfigurationsRequest request, ServerCallContext context)
    {
        try
        {
            var configs = await _mediator.Send(new GetAllMatrixConfigurationsQuery());

            if (configs is null)
                return new GetAllMatrixConfigurationsResponse { Success = false, Message = "Configurations not found" };

            var response = new GetAllMatrixConfigurationsResponse { Success = true };
            foreach (var config in configs)
                response.Configurations.Add(_mapper.Map<MatrixConfigMessage>(config));

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all matrix configurations");
            return new GetAllMatrixConfigurationsResponse { Success = false, Message = "Internal error" };
        }
    }
}
