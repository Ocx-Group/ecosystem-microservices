using Ecosystem.AccountService.Application.DTOs.Ticket;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Ticket;

public record GetAllTicketsByAffiliateIdQuery(int AffiliateId) : IRequest<ICollection<TicketDto>>;
