using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.WalletPeriod;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.WalletService.Application.Handlers.WalletPeriod;

public class IsWithdrawalDateAllowedHandler : IRequestHandler<IsWithdrawalDateAllowedQuery, bool>
{
    private readonly IWalletPeriodRepository _walletPeriodRepository;
    private readonly IConfigurationAdapter _configurationAdapter;

    public IsWithdrawalDateAllowedHandler(
        IWalletPeriodRepository walletPeriodRepository,
        IConfigurationAdapter configurationAdapter)
    {
        _walletPeriodRepository = walletPeriodRepository;
        _configurationAdapter = configurationAdapter;
    }

    public async Task<bool> Handle(IsWithdrawalDateAllowedQuery request, CancellationToken cancellationToken)
    {
        var configuration = await _configurationAdapter.GetBrandConfiguration(
            request.BrandId,
            cancellationToken);
        if (configuration is null)
            return false;

        return configuration.WithdrawalValidationType switch
        {
            "None" => true,
            "DatabaseDriven" => await IsDatabaseDrivenDateAllowed(configuration),
            "FridayUtc" => IsFridayAllowed(configuration),
            _ => false
        };
    }

    private async Task<bool> IsDatabaseDrivenDateAllowed(
        BrandConfigurationDto configuration)
    {
        var timeZone = GetTimeZone(configuration);
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

        if (!IsInsideConfiguredHours(localDateTime.TimeOfDay, configuration))
            return false;

        var allowedDatesObjects = await _walletPeriodRepository.GetAllWalletsPeriods();
        var allowedDates = allowedDatesObjects.Select(wp => wp.Date).ToList();
        var localDateOnly = DateOnly.FromDateTime(localDateTime.Date);
        return allowedDates.Contains(localDateOnly);
    }

    private static bool IsFridayAllowed(BrandConfigurationDto configuration)
    {
        var timeZone = GetTimeZone(configuration);
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);
        return localDateTime.DayOfWeek == DayOfWeek.Friday &&
               IsInsideConfiguredHours(localDateTime.TimeOfDay, configuration);
    }

    private static TimeZoneInfo GetTimeZone(BrandConfigurationDto configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration.WithdrawalTimeZone))
            throw new InvalidOperationException(
                $"Withdrawal time zone is not configured for brand {configuration.BrandId}.");

        return TimeZoneInfo.FindSystemTimeZoneById(configuration.WithdrawalTimeZone);
    }

    private static bool IsInsideConfiguredHours(
        TimeSpan currentTime,
        BrandConfigurationDto configuration)
    {
        if (configuration.WithdrawalStartHour is not int startHour ||
            configuration.WithdrawalEndHour is not int endHour)
        {
            throw new InvalidOperationException(
                $"Withdrawal hours are not configured for brand {configuration.BrandId}.");
        }

        return currentTime >= TimeSpan.FromHours(startHour) &&
               currentTime <= TimeSpan.FromHours(endHour);
    }
}
