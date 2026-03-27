using AutoMapper;
using Ecosystem.WalletService.Application.Queries.InvoiceDetail;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDetailDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.InvoiceDetail;

public class GetAllInvoiceDetailsHandler : IRequestHandler<GetAllInvoiceDetailsQuery, ICollection<InvoiceDetailDto>>
{
    private readonly IInvoiceDetailRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllInvoiceDetailsHandler> _logger;

    public GetAllInvoiceDetailsHandler(
        IInvoiceDetailRepository repository,
        IMapper mapper,
        ILogger<GetAllInvoiceDetailsHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<InvoiceDetailDto>> Handle(GetAllInvoiceDetailsQuery request, CancellationToken cancellationToken)
    {
        var details = await _repository.GetAllInvoiceDetailAsync();
        return _mapper.Map<ICollection<InvoiceDetailDto>>(details);
    }
}
