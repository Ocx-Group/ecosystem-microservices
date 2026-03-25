using AutoMapper;
using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Application.Queries.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdQuery, TicketDto?>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetTicketByIdHandler> _logger;

    public GetTicketByIdHandler(
        ITicketRepository ticketRepository,
        IMapper mapper,
        ILogger<GetTicketByIdHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<TicketDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await _ticketRepository.GetTicketById(request.TicketId);
        return ticket is null ? null : _mapper.Map<TicketDto>(ticket);
    }
}
