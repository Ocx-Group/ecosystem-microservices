using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;

public record UpdatePaymentGroupCommand : IRequest<PaymentGroupsDto?>
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public bool Status { get; init; }
}
