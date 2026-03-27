using System.Text.Json;
using Ecosystem.ConfigurationService.Application.Queries.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public class GetBrandConfigurationByBrandIdHandler
    : IRequestHandler<GetBrandConfigurationByBrandIdQuery, BrandConfigurationDto?>
{
    private readonly IBrandConfigurationRepository _repository;
    private readonly ILogger<GetBrandConfigurationByBrandIdHandler> _logger;

    public GetBrandConfigurationByBrandIdHandler(
        IBrandConfigurationRepository repository,
        ILogger<GetBrandConfigurationByBrandIdHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<BrandConfigurationDto?> Handle(
        GetBrandConfigurationByBrandIdQuery request,
        CancellationToken cancellationToken)
    {
        var entity = await _repository.GetByBrandIdAsync(request.BrandId);
        if (entity is null) return null;

        return MapToDto(entity);
    }

    internal static BrandConfigurationDto MapToDto(Domain.Models.BrandConfiguration entity)
    {
        var commissionLevels = string.IsNullOrEmpty(entity.CommissionLevelsJson)
            ? []
            : JsonSerializer.Deserialize<decimal[]>(entity.CommissionLevelsJson) ?? [];

        return new BrandConfigurationDto
        {
            BrandId = entity.BrandId,
            Name = entity.Brand?.Name ?? string.Empty,
            AdminUserName = entity.AdminUserName,
            SenderName = entity.SenderName,
            SenderEmail = entity.SenderEmail,
            EmailTemplateFolder = entity.EmailTemplateFolder,
            ClientUrl = entity.ClientUrl,
            CommissionEnabled = entity.CommissionEnabled,
            CommissionLevels = commissionLevels,
            BonusPercentage = entity.BonusPercentage,
            PdfTemplateName = entity.PdfTemplateName,
            CompanyName = entity.CompanyName,
            CompanyIdentifier = entity.CompanyIdentifier,
            SupportEmail = entity.SupportEmail,
            SupportPhone = entity.SupportPhone,
            DocumentType = entity.DocumentType,
            LogoUrl = entity.LogoUrl,
            PrimaryColor = entity.PrimaryColor,
            SecondaryColor = entity.SecondaryColor,
            BackgroundColor = entity.BackgroundColor,
            DefaultFatherAffiliateId = entity.DefaultFatherAffiliateId,
            ActivateOnRegistration = entity.ActivateOnRegistration,
            DefaultPaymentGroupId = entity.DefaultPaymentGroupId,
            TradingAcademyPaymentGroupId = entity.TradingAcademyPaymentGroupId,
            WithdrawalValidationType = entity.WithdrawalValidationType,
            WithdrawalTimeZone = entity.WithdrawalTimeZone,
            WithdrawalStartHour = entity.WithdrawalStartHour,
            WithdrawalEndHour = entity.WithdrawalEndHour,
            WithdrawalCapNoDirects = entity.WithdrawalCapNoDirects,
            Requires10PercentPurchaseRule = entity.Requires10PercentPurchaseRule,
            PoolValidationRequired = entity.PoolValidationRequired,
            ConPaymentEnabled = entity.ConPaymentEnabled,
            ConPaymentAddress = entity.ConPaymentAddress,
            BlockchainNetworkId = entity.BlockchainNetworkId,
            IsActive = entity.IsActive
        };
    }
}
