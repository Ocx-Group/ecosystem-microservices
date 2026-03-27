using AutoMapper;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class GetAllInvoicesByUserIdHandler : IRequestHandler<GetAllInvoicesByUserIdQuery, IEnumerable<InvoiceDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetAllInvoicesByUserIdHandler> _logger;

    public GetAllInvoicesByUserIdHandler(
        IInvoiceRepository invoiceRepository,
        IMapper mapper,
        ITenantContext tenantContext,
        ILogger<GetAllInvoicesByUserIdHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _mapper = mapper;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<IEnumerable<InvoiceDto>> Handle(GetAllInvoicesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _invoiceRepository.GetAllInvoicesUser(request.UserId, _tenantContext.TenantId);
        var mappedList = _mapper.Map<IEnumerable<InvoiceDto>>(response).ToList();
        mappedList.Reverse();

        return mappedList;
    }
}
