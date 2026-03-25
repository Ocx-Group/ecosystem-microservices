using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Ticket;

public record MarkTicketMessagesAsReadCommand(int TicketId) : IRequest<bool>;
