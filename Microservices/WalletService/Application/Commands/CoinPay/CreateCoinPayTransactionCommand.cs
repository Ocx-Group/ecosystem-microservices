using Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.CoinPay;

public record CreateCoinPayTransactionCommand : IRequest<CreateTransactionResponse?>
{
    public int AffiliateId { get; init; }
    public string UserName { get; init; } = string.Empty;
    public int Amount { get; init; }
    public List<ProductRequest>? Products { get; init; }
    public int NetworkId { get; init; }
    public int CurrencyId { get; init; }
}
