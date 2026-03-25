using Ecosystem.AccountService.Application.DTOs.Ticket;
using MediatR;

namespace Ecosystem.AccountService.Application.Commands.Ticket;

public record CreateTicketCommand(
    int AffiliateId,
    int? CategoryId,
    string Subject,
    string Description,
    TicketImageItem[]? Images
) : IRequest<TicketDto?>;

public record TicketImageItem(int TicketId, string ImagePath);
