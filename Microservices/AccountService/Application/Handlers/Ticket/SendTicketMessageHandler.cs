using AutoMapper;
using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class SendTicketMessageHandler : IRequestHandler<SendTicketMessageCommand, Unit>
{
    private readonly ITicketMessageRepository _ticketMessageRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SendTicketMessageHandler> _logger;

    public SendTicketMessageHandler(
        ITicketMessageRepository ticketMessageRepository,
        IMapper mapper,
        ILogger<SendTicketMessageHandler> logger)
    {
        _ticketMessageRepository = ticketMessageRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<Unit> Handle(SendTicketMessageCommand request, CancellationToken cancellationToken)
    {
        var message = _mapper.Map<TicketMessage>(request);
        await _ticketMessageRepository.CreateTicketMessage(message);
        return Unit.Value;
    }
}
