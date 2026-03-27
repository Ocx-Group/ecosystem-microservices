using MediatR;

namespace Ecosystem.WalletService.Application.Commands.PaymentTransaction;

public record ConfirmPaymentCommand : IRequest<bool>
{
    public int Id { get; init; }
    public string UserName { get; init; } = string.Empty;
}
