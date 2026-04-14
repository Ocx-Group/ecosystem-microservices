using Ecosystem.WalletService.Application.Queries.WalletPeriod;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.WalletService.Application.Handlers.WalletPeriod;

public class IsWithdrawalDateAllowedHandler : IRequestHandler<IsWithdrawalDateAllowedQuery, bool>
{
    private readonly IWalletPeriodRepository _walletPeriodRepository;

    public IsWithdrawalDateAllowedHandler(IWalletPeriodRepository walletPeriodRepository)
    {
        _walletPeriodRepository = walletPeriodRepository;
    }

    public async Task<bool> Handle(IsWithdrawalDateAllowedQuery request, CancellationToken cancellationToken)
    {
        var defaultZone = Constants.DefaultWithdrawalZone;
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(defaultZone);
        var localDateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZone);

        if (localDateTime.TimeOfDay < new TimeSpan(8, 0, 0) || localDateTime.TimeOfDay > new TimeSpan(18, 0, 0))
            return false;

        var allowedDatesObjects = await _walletPeriodRepository.GetAllWalletsPeriods();
        var allowedDates = allowedDatesObjects.Select(wp => wp.Date).ToList();
        var localDateOnly = DateOnly.FromDateTime(localDateTime.Date);
        return allowedDates.Contains(localDateOnly);
    }
}
