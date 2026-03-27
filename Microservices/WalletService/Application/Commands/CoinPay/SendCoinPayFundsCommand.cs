using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

public record SendCoinPayFundsCommand : IRequest<SendFundsResponse?>
{
    public int IdCurrency { get; init; }
    public int IdNetwork { get; init; }
    public string? Address { get; init; }
    public int Amount { get; init; }
    public string? Details { get; init; }
    public bool AmountPlusFee { get; init; }
}
