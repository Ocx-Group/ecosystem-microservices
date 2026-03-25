using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class CreateTicketHandler : IRequestHandler<CreateTicketCommand, TicketDto?>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateTicketHandler> _logger;

    public CreateTicketHandler(
        ITicketRepository ticketRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateTicketHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TicketDto?> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = _mapper.Map<Domain.Models.Ticket>(request);
        var result = await _ticketRepository.CreateTicket(ticket, _tenantContext.TenantId);

        return result is null ? null : _mapper.Map<TicketDto>(result);
    }
}
