using Ecosystem.AccountService.Application.DTOs.Ticket;
using Ecosystem.AccountService.Application.Queries.Ticket;
using Ecosystem.AccountService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.AccountService.Application.Handlers.Ticket;

public class GetAllTicketSummariesHandler : IRequestHandler<GetAllTicketSummariesQuery, List<TicketSummaryDto>>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ITicketMessageRepository _ticketMessageRepository;
    private readonly ILogger<GetAllTicketSummariesHandler> _logger;

    public GetAllTicketSummariesHandler(
        ITicketRepository ticketRepository,
        ITicketMessageRepository ticketMessageRepository,
        ILogger<GetAllTicketSummariesHandler> logger)
    {
        _ticketRepository = ticketRepository;
        _ticketMessageRepository = ticketMessageRepository;
        _logger = logger;
    }

    public async Task<List<TicketSummaryDto>> Handle(GetAllTicketSummariesQuery request, CancellationToken cancellationToken)
    {
        var tickets = await _ticketRepository.GetAllTickets(request.BrandId);
        var ticketSummaries = new List<TicketSummaryDto>();

        foreach (var ticket in tickets)
        {
            var allMessages = await _ticketMessageRepository.GetMessagesByTicketId(ticket.Id);
            var unreadMessagesCount = allMessages.Count(m => !m.IsRead);

            if (unreadMessagesCount > 0)
            {
                var lastMessage = allMessages.MaxBy(m => m.CreatedAt);
                ticketSummaries.Add(new TicketSummaryDto
                {
                    TicketId = ticket.Id,
                    Title = ticket.Subject,
                    UnreadMessagesCount = unreadMessagesCount,
                    LastUpdated = lastMessage?.CreatedAt ?? ticket.CreatedAt
                });
            }
        }

        return ticketSummaries;
    }
}
