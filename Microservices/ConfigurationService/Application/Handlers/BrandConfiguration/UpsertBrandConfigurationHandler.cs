using System.Text.Json;
using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public class UpsertBrandConfigurationHandler
    : IRequestHandler<UpsertBrandConfigurationCommand, BrandConfigurationDto>
{
    private readonly IBrandConfigurationRepository _repository;
    private readonly IBrandConfigurationProvider _brandConfigProvider;
    private readonly ILogger<UpsertBrandConfigurationHandler> _logger;

    public UpsertBrandConfigurationHandler(
        IBrandConfigurationRepository repository,
        IBrandConfigurationProvider brandConfigProvider,
        ILogger<UpsertBrandConfigurationHandler> logger)
    {
        _repository = repository;
        _brandConfigProvider = brandConfigProvider;
        _logger = logger;
    }

    public async Task<BrandConfigurationDto> Handle(
        UpsertBrandConfigurationCommand request,
        CancellationToken cancellationToken)
    {
        var entity = new Domain.Models.BrandConfiguration
        {
            BrandId = request.BrandId,
            AdminUserName = request.AdminUserName,
            SenderName = request.SenderName,
            SenderEmail = request.SenderEmail,
            EmailTemplateFolder = request.EmailTemplateFolder,
            ClientUrl = request.ClientUrl,
            CommissionEnabled = request.CommissionEnabled,
            CommissionLevelsJson = JsonSerializer.Serialize(request.CommissionLevels),
            BonusPercentage = request.BonusPercentage,
            PdfTemplateName = request.PdfTemplateName,
            CompanyName = request.CompanyName,
            CompanyIdentifier = request.CompanyIdentifier,
            SupportEmail = request.SupportEmail,
            SupportPhone = request.SupportPhone,
            DocumentType = request.DocumentType,
            LogoUrl = request.LogoUrl,
            PrimaryColor = request.PrimaryColor,
            SecondaryColor = request.SecondaryColor,
            BackgroundColor = request.BackgroundColor,
            DefaultFatherAffiliateId = request.DefaultFatherAffiliateId,
            ActivateOnRegistration = request.ActivateOnRegistration,
            DefaultPaymentGroupId = request.DefaultPaymentGroupId,
            TradingAcademyPaymentGroupId = request.TradingAcademyPaymentGroupId,
            WithdrawalValidationType = request.WithdrawalValidationType,
            WithdrawalTimeZone = request.WithdrawalTimeZone,
            WithdrawalStartHour = request.WithdrawalStartHour,
            WithdrawalEndHour = request.WithdrawalEndHour,
            WithdrawalCapNoDirects = request.WithdrawalCapNoDirects,
            Requires10PercentPurchaseRule = request.Requires10PercentPurchaseRule,
            PoolValidationRequired = request.PoolValidationRequired,
            ConPaymentEnabled = request.ConPaymentEnabled,
            ConPaymentAddress = request.ConPaymentAddress,
            BlockchainNetworkId = request.BlockchainNetworkId,
            IsActive = request.IsActive
        };

        var saved = await _repository.UpsertAsync(entity);

        // Invalidate cache so all services pick up the change
        await _brandConfigProvider.InvalidateCacheAsync(request.BrandId);

        _logger.LogInformation("Brand configuration upserted for BrandId {BrandId}", request.BrandId);

        return GetBrandConfigurationByBrandIdHandler.MapToDto(saved);
    }
}
