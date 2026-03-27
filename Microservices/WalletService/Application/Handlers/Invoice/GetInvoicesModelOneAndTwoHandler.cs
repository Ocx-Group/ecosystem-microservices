using AutoMapper;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class GetInvoicesModelOneAndTwoHandler : IRequestHandler<GetInvoicesModelOneAndTwoQuery, IEnumerable<InvoiceModelOneAndTwoDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetInvoicesModelOneAndTwoHandler> _logger;

    public GetInvoicesModelOneAndTwoHandler(
        IInvoiceRepository invoiceRepository,
        IMapper mapper,
        ILogger<GetInvoicesModelOneAndTwoHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<InvoiceModelOneAndTwoDto>> Handle(GetInvoicesModelOneAndTwoQuery request, CancellationToken cancellationToken)
    {
        var response = await _invoiceRepository.GetAllInvoicesModelOneAndTwo();

        if (response is null)
            return new List<InvoiceModelOneAndTwoDto>();

        var invoices = _mapper.Map<IEnumerable<InvoiceModelOneAndTwoDto>>(response);
        var invoicesOrdered = invoices.OrderByDescending(e => e.CreatedAt).ToList();

        return invoicesOrdered;
    }
}
