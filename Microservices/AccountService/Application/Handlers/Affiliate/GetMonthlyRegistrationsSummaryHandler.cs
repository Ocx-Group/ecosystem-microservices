using Ecosystem.AccountService.Application.DTOs;
using Ecosystem.AccountService.Application.Queries.Affiliate;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;

namespace Ecosystem.AccountService.Application.Handlers.Affiliate;

public class GetMonthlyRegistrationsSummaryHandler
    : IRequestHandler<GetMonthlyRegistrationsSummaryQuery, IEnumerable<MonthlyRegistrationsDto>>
{
    private readonly IUserAffiliateInfoRepository _repo;
    private readonly ITenantContext _tenantContext;

    public GetMonthlyRegistrationsSummaryHandler(IUserAffiliateInfoRepository repo, ITenantContext tenantContext)
    {
        _repo = repo;
        _tenantContext = tenantContext;
    }

    public async Task<IEnumerable<MonthlyRegistrationsDto>> Handle(GetMonthlyRegistrationsSummaryQuery request,
        CancellationToken ct)
    {
        var months = request.Months;
        var now = DateTime.UtcNow;
        var firstMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-(months - 1));

        var totals = await _repo.GetMonthlyRegistrationsTotals(_tenantContext.TenantId, firstMonth);
        var totalsByMonth = totals.ToDictionary(x => (x.Year, x.Month), x => x.Total);

        var summary = new List<MonthlyRegistrationsDto>(months);

        for (var i = 0; i < months; i++)
        {
            var month = firstMonth.AddMonths(i);

            summary.Add(new MonthlyRegistrationsDto
            {
                Year = month.Year,
                Month = month.Month,
                Total = totalsByMonth.TryGetValue((month.Year, month.Month), out var total) ? total : 0
            });
        }

        return summary;
    }
}
