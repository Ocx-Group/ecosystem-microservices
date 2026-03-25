using Ecosystem.AccountService.Application.Commands.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class DeleteTicketsHandler : IRequestHandler<DeleteTicketsCommand, bool>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ILogger<DeleteTicketsHandler> _logger;

    public DeleteTicketsHandler(
        ITicketRepository ticketRepository,
        ILogger<DeleteTicketsHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteTicketsCommand request, CancellationToken cancellationToken)
    {
        var tickets = new List<Domain.Models.Ticket>();
        var images = new List<Domain.Models.TicketImage>();
        var messages = new List<Domain.Models.TicketMessage>();

        foreach (var ticketId in request.TicketIds)
        {
            var ticket = await _ticketRepository.GetTicketById(ticketId);
            var imagesList = await _ticketRepository.GetImagesByTicketId(ticketId);
            var messagesList = await _ticketRepository.GetMessagesByTicketId(ticketId);

            if (ticket != null)
            {
                tickets.Add(ticket);
                if (imagesList != null) images.AddRange(imagesList);
                if (messagesList != null) messages.AddRange(messagesList);
            }
        }

        if (!tickets.Any())
            return false;

        await _ticketRepository.DeleteTickets(tickets);
        await _ticketRepository.DeleteImages(images);
        await _ticketRepository.DeleteMessages(messages);
        return true;
    }
}
