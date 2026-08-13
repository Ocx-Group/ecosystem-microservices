using AutoMapper;
using Ecosystem.ConfigurationService.Application.Queries.MatrixConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Grpc.Configuration;
using Grpc.Core;
using MediatR;

namespace Ecosystem.ConfigurationService.Api.GrpcServices;

public class ConfigurationGrpcService : ConfigurationGrpc.ConfigurationGrpcBase
{
    private readonly IPdfTemplateRepository _pdfTemplateRepository;
    private readonly IBrandConfigurationProvider _brandConfigurationProvider;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    private readonly ILogger<ConfigurationGrpcService> _logger;

    public ConfigurationGrpcService(
        IPdfTemplateRepository pdfTemplateRepository,
        IBrandConfigurationProvider brandConfigurationProvider,
        IMediator mediator,
        IMapper mapper,
        ILogger<ConfigurationGrpcService> logger)
    {
        _pdfTemplateRepository = pdfTemplateRepository;
        _brandConfigurationProvider = brandConfigurationProvider;
        _mediator = mediator;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<GetBrandConfigurationResponse> GetBrandConfiguration(
        GetBrandConfigurationRequest request, ServerCallContext context)
    {
        try
        {
            var configuration = await _brandConfigurationProvider.GetByBrandIdAsync(request.BrandId);

            if (configuration is null || !configuration.IsActive)
            {
                return new GetBrandConfigurationResponse
                {
                    Success = false,
                    Message = $"Active brand configuration not found for brand {request.BrandId}"
                };
            }

            return new GetBrandConfigurationResponse
            {
                Success = true,
                Configuration = ToGrpcMessage(configuration)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching brand configuration for BrandId {BrandId}", request.BrandId);
            throw new RpcException(
                new Status(StatusCode.Internal, "Internal error retrieving brand configuration"));
        }
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

    private static BrandConfigurationMessage ToGrpcMessage(BrandConfigurationDto source)
    {
        var message = new BrandConfigurationMessage
        {
            BrandId = source.BrandId,
            Name = source.Name,
            SenderName = source.SenderName,
            SenderEmail = source.SenderEmail,
            ClientUrl = source.ClientUrl,
            CompanyName = source.CompanyName,
            SupportEmail = source.SupportEmail,
            PrimaryColor = source.PrimaryColor,
            SecondaryColor = source.SecondaryColor,
            BackgroundColor = source.BackgroundColor,
            IsActive = source.IsActive,
            AdminUserName = source.AdminUserName,
            EmailTemplateFolder = source.EmailTemplateFolder,
            CommissionEnabled = source.CommissionEnabled,
            BonusPercentage = decimal.ToDouble(source.BonusPercentage),
            DailyBonusAlwaysDistribute = source.DailyBonusAlwaysDistribute,
            MonthlyCommissionEnabled = source.MonthlyCommissionEnabled,
            MonthlyCommissionInterestRate = decimal.ToDouble(source.MonthlyCommissionInterestRate),
            MonthlyCommissionWaitingDays = source.MonthlyCommissionWaitingDays,
            PdfTemplateName = source.PdfTemplateName,
            ActivateOnRegistration = source.ActivateOnRegistration,
            WithdrawalValidationType = source.WithdrawalValidationType,
            Requires10PercentPurchaseRule = source.Requires10PercentPurchaseRule,
            PoolValidationRequired = source.PoolValidationRequired,
            ConPaymentEnabled = source.ConPaymentEnabled
        };

        message.CommissionLevels.AddRange(
            source.CommissionLevels.Select(decimal.ToDouble));
        if (source.CompanyIdentifier is not null) message.CompanyIdentifier = source.CompanyIdentifier;
        if (source.SupportPhone is not null) message.SupportPhone = source.SupportPhone;
        if (source.DocumentType is not null) message.DocumentType = source.DocumentType;
        if (source.LogoUrl is not null) message.LogoUrl = source.LogoUrl;
        if (source.DefaultFatherAffiliateId is not null)
            message.DefaultFatherAffiliateId = source.DefaultFatherAffiliateId.Value;
        if (source.DefaultPaymentGroupId is not null)
            message.DefaultPaymentGroupId = source.DefaultPaymentGroupId.Value;
        if (source.TradingAcademyPaymentGroupId is not null)
            message.TradingAcademyPaymentGroupId = source.TradingAcademyPaymentGroupId.Value;
        if (source.MonthlyCommissionPaymentGroupId is not null)
            message.MonthlyCommissionPaymentGroupId = source.MonthlyCommissionPaymentGroupId.Value;
        if (source.WithdrawalTimeZone is not null)
            message.WithdrawalTimeZone = source.WithdrawalTimeZone;
        if (source.WithdrawalStartHour is not null)
            message.WithdrawalStartHour = source.WithdrawalStartHour.Value;
        if (source.WithdrawalEndHour is not null)
            message.WithdrawalEndHour = source.WithdrawalEndHour.Value;
        if (source.WithdrawalCapNoDirects is not null)
            message.WithdrawalCapNoDirects = decimal.ToDouble(source.WithdrawalCapNoDirects.Value);
        if (source.ConPaymentAddress is not null)
            message.ConPaymentAddress = source.ConPaymentAddress;
        if (source.BlockchainNetworkId is not null)
            message.BlockchainNetworkId = source.BlockchainNetworkId.Value;

        return message;
    }
}
