using Ecosystem.ConfigurationService.Application.Commands.BrandConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.BrandConfiguration;

public sealed class UpdateOwnMonthlyCommissionSettingsHandler(
    IBrandConfigurationRepository repository,
    IBrandConfigurationProvider brandConfigurationProvider,
    ITenantContext tenantContext,
    ILogger<UpdateOwnMonthlyCommissionSettingsHandler> logger)
    : IRequestHandler<UpdateOwnMonthlyCommissionSettingsCommand, UpdateMonthlyCommissionSettingsResult>
{
    public async Task<UpdateMonthlyCommissionSettingsResult> Handle(
        UpdateOwnMonthlyCommissionSettingsCommand request,
        CancellationToken cancellationToken)
    {
        var brandId = GetOwnBrandingHandler.RequireTenant(tenantContext);
        var settings = request.Settings;

        // `monthly_commission_interest_rate` is a NUMERIC(5,2), so anything finer is
        // rounded away on write. Rounding first keeps what is configured, what is
        // displayed and what the liquidation actually pays identical, and makes the
        // rules below apply to the exact value that will be used.
        var interestRate = Math.Round(
            settings.InterestRate, 2, MidpointRounding.AwayFromZero);

        var validationMessage = Validate(
            settings.Enabled, interestRate, settings.WaitingDays, settings.PaymentGroupId);

        if (validationMessage is not null)
        {
            logger.LogWarning(
                "Monthly commission settings update for BrandId {BrandId} rejected: {Reason}",
                brandId,
                validationMessage);
            return new UpdateMonthlyCommissionSettingsResult(
                UpdateMonthlyCommissionSettingsStatus.InvalidSettings, null, validationMessage);
        }

        var saved = await repository.UpdateMonthlyCommissionSettingsAsync(
            brandId,
            settings.Enabled,
            interestRate,
            settings.WaitingDays,
            settings.PaymentGroupId);

        if (saved is null)
            return new UpdateMonthlyCommissionSettingsResult(
                UpdateMonthlyCommissionSettingsStatus.NotFound, null);

        // WalletService resolves these from the cached brand configuration on every
        // liquidation. Without this the previous rate keeps being paid until the cache
        // expires on its own.
        await brandConfigurationProvider.InvalidateCacheAsync(brandId);

        logger.LogInformation(
            "Monthly commission settings updated for BrandId {BrandId} by admin user {ActorUserId} ({ActorUserName}): "
            + "enabled={Enabled}, rate={InterestRate}, waitingDays={WaitingDays}, paymentGroup={PaymentGroupId}",
            brandId,
            request.ActorUserId,
            request.ActorUserName,
            settings.Enabled,
            interestRate,
            settings.WaitingDays,
            settings.PaymentGroupId);

        return new UpdateMonthlyCommissionSettingsResult(
            UpdateMonthlyCommissionSettingsStatus.Updated,
            GetOwnMonthlyCommissionSettingsHandler.Map(saved));
    }

    /// <summary>
    /// Returns the reason the payload is unusable, or <c>null</c> when it is valid.
    /// Every rule protects a real payout: these values are passed straight to
    /// <c>calculate_monthly_commissions</c>, which credits wallets.
    /// </summary>
    private static string? Validate(
        bool enabled, decimal interestRate, int waitingDays, int? paymentGroupId)
    {
        if (interestRate < 0)
            return "The interest rate cannot be negative.";

        if (interestRate > MonthlyCommissionSettingsLimits.MaxInterestRate)
            return $"The interest rate cannot exceed {MonthlyCommissionSettingsLimits.MaxInterestRate}%.";

        if (waitingDays < 0)
            return "The waiting days cannot be negative.";

        if (waitingDays > MonthlyCommissionSettingsLimits.MaxWaitingDays)
            return $"The waiting days cannot exceed {MonthlyCommissionSettingsLimits.MaxWaitingDays}.";

        if (paymentGroupId is <= 0)
            return "The payment group must be a positive identifier.";

        // Without a payment group the function has nothing to select invoices by, so
        // enabling the feature would either pay nothing or fault at run time.
        if (enabled && paymentGroupId is null)
            return "A payment group is required while the monthly commission is enabled.";

        if (enabled && interestRate == 0)
            return "The interest rate must be greater than zero while the monthly commission is enabled.";

        return null;
    }
}
