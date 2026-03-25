using Ecosystem.AccountService.Application.DTOs.Ticket;
using MediatR;

namespace Ecosystem.AccountService.Application.Queries.Ticket;

public record GetAllTicketSummariesQuery(int BrandId) : IRequest<List<TicketSummaryDto>>;
