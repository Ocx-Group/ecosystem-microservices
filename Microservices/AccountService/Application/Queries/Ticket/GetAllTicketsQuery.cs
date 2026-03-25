using Ecosystem.AccountService.Application.DTOs.Ticket;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Ticket;

public record GetAllTicketsQuery(int BrandId) : IRequest<ICollection<TicketDto>>;
