using AutoMapper;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.Extensions;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class GetTradingAcademyInvoicesHandler : IRequestHandler<GetTradingAcademyInvoicesQuery, IEnumerable<InvoiceTradingAcademyDto?>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTradingAcademyInvoicesHandler> _logger;

    public GetTradingAcademyInvoicesHandler(
        IInvoiceRepository invoiceRepository,
        IMapper mapper,
        ILogger<GetTradingAcademyInvoicesHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<InvoiceTradingAcademyDto?>> Handle(GetTradingAcademyInvoicesQuery request, CancellationToken cancellationToken)
    {
        var response = await _invoiceRepository.GetAllInvoicesForTradingAcademyPurchases();

        if (response is null)
            return new List<InvoiceTradingAcademyDto>();

        var mappedList = _mapper.Map<List<InvoiceTradingAcademyDto>>(response);

        if (mappedList is null)
            return new List<InvoiceTradingAcademyDto>();

        foreach (var invoice in mappedList)
        {
            if (invoice.ProductId == 31)
            {
                var (startDate, endDate) = CommonExtensions.CalculateMonthlyCourseDates(invoice.CreatedAt);
                invoice.StartDay = startDate.Date.ToString("yyyy-MM-dd");
                invoice.EndDay = endDate.Date.ToString("yyyy-MM-dd");
            }
            else if (invoice.ProductId == 32)
            {
                var (startDate, endDate) = CommonExtensions.CalculateWeeklyCourseDates(invoice.CreatedAt);
                invoice.StartDay = startDate.Date.ToString("yyyy-MM-dd");
                invoice.EndDay = endDate.Date.ToString("yyyy-MM-dd");
            }
        }

        return mappedList;
    }
}
