using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.PaymentGroup;

public record CreatePaymentGroupCommand : IRequest<PaymentGroupsDto>
{
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public bool Status { get; init; }
}
