using Ecosystem.WalletService.Domain.Responses.BaseResponses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

public record CreateCoinPayAddressCommand : IRequest<CreateAddressResponse?>
{
    public int IdCurrency { get; init; }
    public int IdNetwork { get; init; }
    public int IdWallet { get; init; }
}
