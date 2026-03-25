using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class MarkTicketMessagesAsReadHandler : IRequestHandler<MarkTicketMessagesAsReadCommand, bool>
{
    private readonly ITicketMessageRepository _ticketMessageRepository;
    private readonly ILogger<MarkTicketMessagesAsReadHandler> _logger;

    public MarkTicketMessagesAsReadHandler(
        ITicketMessageRepository ticketMessageRepository,
        ILogger<MarkTicketMessagesAsReadHandler> logger)
    {
        _ticketMessageRepository = ticketMessageRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(MarkTicketMessagesAsReadCommand request, CancellationToken cancellationToken)
    {
        var updatedMessages = await _ticketMessageRepository.UpdateMessagesByTicketId(request.TicketId);
        return updatedMessages.Any();
    }
}
