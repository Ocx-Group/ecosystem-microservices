using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class GetMonthlyPurchasesSummaryHandler
    : IRequestHandler<GetMonthlyPurchasesSummaryQuery, IEnumerable<MonthlyPurchasesDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly ITenantContext _tenantContext;

    public GetMonthlyPurchasesSummaryHandler(IInvoiceRepository invoiceRepository, ITenantContext tenantContext)
    {
        _invoiceRepository = invoiceRepository;
        _tenantContext = tenantContext;
    }

    public async Task<IEnumerable<MonthlyPurchasesDto>> Handle(GetMonthlyPurchasesSummaryQuery query,
        CancellationToken cancellationToken)
    {
        var months = query.Months;
        var now = DateTime.UtcNow;
        var firstMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-(months - 1));

        var totals = await _invoiceRepository.GetMonthlyPurchasesTotals(_tenantContext.TenantId, firstMonth);
        var totalsByMonth = totals.ToDictionary(x => (x.Year, x.Month));

        var summary = new List<MonthlyPurchasesDto>(months);

        for (var i = 0; i < months; i++)
        {
            var month = firstMonth.AddMonths(i);

            summary.Add(totalsByMonth.TryGetValue((month.Year, month.Month), out var total)
                ? total
                : new MonthlyPurchasesDto { Year = month.Year, Month = month.Month, TotalAmount = 0 });
        }

        return summary;
    }
}
