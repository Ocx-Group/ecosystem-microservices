using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class UpdateOwnCommissionSettingsHandler(
    IBrandConfigurationRepository repository,
    IBrandConfigurationProvider brandConfigurationProvider,
    ITenantContext tenantContext,
    ILogger<UpdateOwnCommissionSettingsHandler> logger)
    : IRequestHandler<UpdateOwnCommissionSettingsCommand, UpdateCommissionSettingsResult>
{
    public async Task<UpdateCommissionSettingsResult> Handle(
        UpdateOwnCommissionSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var brandId = GetOwnBrandingHandler.RequireTenant(tenantContext);
        var settings = request.Settings;

        // `distribute_commissions_per_purchase` holds each percentage in a
        // NUMERIC(5,2), so anything finer is rounded away at payout time. Rounding
        // first keeps what is configured, displayed and actually paid equal, and makes
        // the rules below apply to the exact values that will be credited.
        var levels = Normalize(settings.CommissionLevels);

        var validationMessage = Validate(settings.CommissionEnabled, levels);
        if (validationMessage is not null)
        {
            logger.LogWarning(
                "Commission settings update for BrandId {BrandId} rejected: {Reason}",
                brandId,
                validationMessage);
            return new UpdateCommissionSettingsResult(
                UpdateCommissionSettingsStatus.InvalidLevels, null, validationMessage);
        }

        var saved = await repository.UpdateCommissionSettingsAsync(
            brandId,
            settings.CommissionEnabled,
            levels,
            settings.DailyBonusAlwaysDistribute);

        if (saved is null)
            return new UpdateCommissionSettingsResult(
                UpdateCommissionSettingsStatus.NotFound, null);

        // WalletService reads these percentages from the cached brand configuration on
        // every purchase. Without this the old rates keep being paid until the cache
        // expires on its own.
        await brandConfigurationProvider.InvalidateCacheAsync(brandId);

        logger.LogInformation(
            "Commission settings updated for BrandId {BrandId} by admin user {ActorUserId} ({ActorUserName}): "
            + "enabled={CommissionEnabled}, levels={LevelCount}, alwaysDistribute={AlwaysDistribute}",
            brandId,
            request.ActorUserId,
            request.ActorUserName,
            settings.CommissionEnabled,
            settings.CommissionLevels.Length,
            settings.DailyBonusAlwaysDistribute);

        return new UpdateCommissionSettingsResult(
            UpdateCommissionSettingsStatus.Updated,
            GetOwnCommissionSettingsHandler.Map(saved));
    }

    /// <summary>
    /// Rounds half away from zero, which is what PostgreSQL does when narrowing to
    /// NUMERIC(5,2), so the stored value matches the one the payout will use.
    /// </summary>
    private static decimal[] Normalize(decimal[] levels)
        => levels
            .Select(percentage => Math.Round(percentage, 2, MidpointRounding.AwayFromZero))
            .ToArray();

    /// <summary>
    /// Returns the reason the payload is unusable, or <c>null</c> when it is valid.
    /// Every rule protects a real payout: the levels feed
    /// <c>wallet_service.distribute_commissions_per_purchase</c> straight from here.
    /// </summary>
    private static string? Validate(bool commissionEnabled, decimal[] levels)
    {
        if (levels.Length > CommissionSettingsLimits.MaxLevels)
            return $"At most {CommissionSettingsLimits.MaxLevels} levels can be configured.";

        if (commissionEnabled && levels.Length == 0)
            return "At least one level is required while the commission is enabled.";

        foreach (var percentage in levels)
        {
            if (percentage < 0)
                return "Level percentages cannot be negative.";

            if (percentage > CommissionSettingsLimits.MaxLevelPercentage)
                return $"A level cannot exceed {CommissionSettingsLimits.MaxLevelPercentage}%.";
        }

        var total = levels.Sum();
        if (total > CommissionSettingsLimits.MaxTotalPercentage)
            return $"The levels add up to {total}%, which exceeds the {CommissionSettingsLimits.MaxTotalPercentage}% allowed.";

        return null;
    }
}
