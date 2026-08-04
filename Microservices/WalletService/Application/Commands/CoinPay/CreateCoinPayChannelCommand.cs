using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

public record CreateCoinPayChannelCommand : IRequest<CreateChannelResponse?>
{
    public int AffiliateId { get; init; }
    public int Amount { get; init; }
    public List<ProductRequest>? Products { get; init; }
    public int IdCurrency { get; init; }
    public int IdNetwork { get; init; }
    public string? TagName { get; init; }
}
