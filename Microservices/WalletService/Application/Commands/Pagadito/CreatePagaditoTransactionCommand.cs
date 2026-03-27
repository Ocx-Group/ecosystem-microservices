using Ecosystem.WalletService.Domain.Requests.PagaditoRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Pagadito;

public record CreatePagaditoTransactionCommand : IRequest<string?>
{
    public decimal Amount { get; init; }
    public int AffiliateId { get; init; }
    public List<PagaditoTransactionDetailRequest> Details { get; init; } = new();
    public Dictionary<string, string> CustomParams { get; init; } = new();
}
