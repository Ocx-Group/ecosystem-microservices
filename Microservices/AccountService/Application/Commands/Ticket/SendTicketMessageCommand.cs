using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Ticket;

public record SendTicketMessageCommand(
    int TicketId,
    int UserId,
    string MessageContent,
    string UserName,
    string ImageProfileUrl
) : IRequest<Unit>;
