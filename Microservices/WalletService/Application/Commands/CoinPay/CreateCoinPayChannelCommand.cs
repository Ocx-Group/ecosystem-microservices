using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

public record CreateCoinPayChannelCommand : IRequest<CreateChannelResponse?>
{
    public int IdCurrency { get; init; }
    public int IdNetwork { get; init; }
    public int IdExternalIdentification { get; init; }
    public string? TagName { get; init; }
}
