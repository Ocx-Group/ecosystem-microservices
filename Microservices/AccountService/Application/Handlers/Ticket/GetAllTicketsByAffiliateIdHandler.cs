using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Application.Queries.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class GetAllTicketsByAffiliateIdHandler : IRequestHandler<GetAllTicketsByAffiliateIdQuery, ICollection<TicketDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllTicketsByAffiliateIdHandler> _logger;

    public GetAllTicketsByAffiliateIdHandler(
        ITicketRepository ticketRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllTicketsByAffiliateIdHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<TicketDto>> Handle(GetAllTicketsByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _ticketRepository.GetAllTicketsByAffiliateId(request.AffiliateId, _tenantContext.TenantId);
        return _mapper.Map<ICollection<TicketDto>>(result);
    }
}
