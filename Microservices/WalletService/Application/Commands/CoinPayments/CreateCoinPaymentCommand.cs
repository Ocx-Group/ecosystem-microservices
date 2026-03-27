using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPayments;

public record CreateCoinPaymentCommand : IRequest<CreateConPaymentsTransactionResponse?>
{
    public decimal Amount { get; init; }
    public string Currency1 { get; init; } = string.Empty;
    public string Currency2 { get; init; } = string.Empty;
    public string BuyerEmail { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string? BuyerName { get; init; }
    public string? ItemName { get; init; }
    public string? ItemNumber { get; init; }
    public string? Invoice { get; init; }
    public string? Custom { get; init; }
    public string? IpnUrl { get; init; }
    public string? SuccessUrl { get; init; }
    public string? CancelUrl { get; init; }
    public List<ProductRequest> Products { get; init; } = new();
}
