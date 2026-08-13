using Ecosystem.Domain.Core.Caching;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.MonthlyCommissionDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

/// <summary>
/// Drives the monthly liquidation for the authenticated brand.
///
/// Every parameter that decides who gets paid is resolved here rather than trusted from
/// the request: the brand comes from the tenant, the administrator name from the brand
/// configuration, and the rate, waiting days and payment group default to the brand's
/// own settings. The screen may override the last three for a single run, which is why
/// they are nullable on the request.
/// </summary>
public class CalculateMonthlyCommissionsHandler
    : IRequestHandler<CalculateMonthlyCommissionsCommand, CalculateMonthlyCommissionsResult>
{
    private readonly IWalletRepository _walletRepository;
    private readonly IConfigurationAdapter _configurationAdapter;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CalculateMonthlyCommissionsHandler> _logger;

    public CalculateMonthlyCommissionsHandler(
        IWalletRepository walletRepository,
        IConfigurationAdapter configurationAdapter,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<CalculateMonthlyCommissionsHandler> logger)
    {
        _walletRepository = walletRepository;
        _configurationAdapter = configurationAdapter;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<CalculateMonthlyCommissionsResult> Handle(
        CalculateMonthlyCommissionsCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;
        var brandId = _tenantContext.TenantId;

        if (brandId <= 0)
            throw new UnauthorizedAccessException("Authenticated brand context is required.");

        var brandConfiguration =
            await _configurationAdapter.GetBrandConfiguration(brandId, cancellationToken);

        if (brandConfiguration is null)
            return new CalculateMonthlyCommissionsResult(
                CalculateMonthlyCommissionsStatus.BrandNotFound, null);

        // The switch is the brand's own kill switch. Overriding the parameters per run
        // must not become a way around a brand that has deliberately turned the
        // liquidation off.
        if (!brandConfiguration.MonthlyCommissionEnabled)
            return new CalculateMonthlyCommissionsResult(
                CalculateMonthlyCommissionsStatus.Disabled,
                null,
                "The monthly commission is disabled for this brand.");

        var interestRate = request.InterestRate
            ?? brandConfiguration.MonthlyCommissionInterestRate;
        var waitingDays = request.WaitingDays
            ?? brandConfiguration.MonthlyCommissionWaitingDays;
        var paymentGroupId = request.PaymentGroupId
            ?? brandConfiguration.MonthlyCommissionPaymentGroupId;

        var validationMessage = Validate(
            request.StartDate, request.EndDate, interestRate, waitingDays, paymentGroupId);

        if (validationMessage is not null)
        {
            _logger.LogWarning(
                "Monthly commission run for BrandId {BrandId} rejected: {Reason}",
                brandId,
                validationMessage);
            return new CalculateMonthlyCommissionsResult(
                CalculateMonthlyCommissionsStatus.InvalidRequest, null, validationMessage);
        }

        var items = await _walletRepository.CalculateMonthlyCommissionsAsync(
            request.StartDate,
            request.EndDate,
            interestRate,
            waitingDays,
            paymentGroupId!.Value,
            brandConfiguration.AdminUserName,
            brandId,
            request.DryRun);

        var result = new MonthlyCommissionResultDto
        {
            DryRun = request.DryRun,
            RowsAffected = items.Count,
            TotalCredit = items.Sum(item => item.Credit),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            InterestRate = interestRate,
            WaitingDays = waitingDays,
            PaymentGroupId = paymentGroupId.Value,
            Items = items
        };

        if (request.DryRun)
        {
            _logger.LogInformation(
                "Monthly commission simulated for BrandId {BrandId} by admin user {ActorUserId} ({ActorUserName}): "
                + "period {StartDate}..{EndDate}, rate={InterestRate}, waitingDays={WaitingDays}, "
                + "paymentGroup={PaymentGroupId} — {RowsAffected} affiliates, {TotalCredit} total",
                brandId, command.ActorUserId, command.ActorUserName,
                request.StartDate, request.EndDate, interestRate, waitingDays,
                paymentGroupId.Value, result.RowsAffected, result.TotalCredit);

            return new CalculateMonthlyCommissionsResult(
                CalculateMonthlyCommissionsStatus.Completed, result);
        }

        // The balances just changed. Without this the affiliate keeps seeing the
        // pre-liquidation figure until the cached entry expires on its own.
        await _cacheService.InvalidateBalanceAsync(
            items.Select(item => item.AffiliateId).ToArray());

        _logger.LogInformation(
            "Monthly commission liquidated for BrandId {BrandId} by admin user {ActorUserId} ({ActorUserName}): "
            + "period {StartDate}..{EndDate}, rate={InterestRate}, waitingDays={WaitingDays}, "
            + "paymentGroup={PaymentGroupId} — {RowsAffected} affiliates credited, {TotalCredit} total",
            brandId, command.ActorUserId, command.ActorUserName,
            request.StartDate, request.EndDate, interestRate, waitingDays,
            paymentGroupId.Value, result.RowsAffected, result.TotalCredit);

        return new CalculateMonthlyCommissionsResult(
            CalculateMonthlyCommissionsStatus.Completed, result);
    }

    /// <summary>
    /// Returns the reason the run cannot proceed, or <c>null</c> when it can. These are
    /// the mistakes that would silently pay the wrong amount rather than fail loudly.
    /// </summary>
    private static string? Validate(
        DateOnly startDate,
        DateOnly endDate,
        decimal interestRate,
        int waitingDays,
        int? paymentGroupId)
    {
        if (startDate > endDate)
            return "The start date cannot be after the end date.";

        // A period that has not finished yet would pay a partial month at full rate the
        // first time and then be locked out by the idempotency key.
        if (endDate >= DateOnly.FromDateTime(DateTime.UtcNow.Date))
            return "The period must be over before it can be liquidated.";

        var days = endDate.DayNumber - startDate.DayNumber + 1;
        if (days > MonthlyCommissionConstants.MaxPeriodDays)
            return $"The period cannot be longer than {MonthlyCommissionConstants.MaxPeriodDays} days.";

        if (interestRate <= 0)
            return "The interest rate must be greater than zero.";

        if (interestRate > 100)
            return "The interest rate cannot exceed 100%.";

        if (waitingDays < 0)
            return "The waiting days cannot be negative.";

        if (waitingDays > days)
            return "The waiting days cannot exceed the length of the period.";

        if (paymentGroupId is null or <= 0)
            return "A payment group is required to liquidate.";

        return null;
    }
}
