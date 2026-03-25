using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Ticket;

public record DeleteTicketsCommand(List<int> TicketIds) : IRequest<bool>;
