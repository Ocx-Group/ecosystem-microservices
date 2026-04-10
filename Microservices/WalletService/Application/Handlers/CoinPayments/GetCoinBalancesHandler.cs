using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPayments;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class GetCoinBalancesHandler : IRequestHandler<GetCoinBalancesQuery, GetCoinBalancesResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;

    public GetCoinBalancesHandler(
        ICoinPaymentsAdapter adapter,
        ILogger<GetCoinBalancesHandler> logger)
    {
        _adapter = adapter;
    }

    public async Task<GetCoinBalancesResponse?> Handle(GetCoinBalancesQuery request, CancellationToken cancellationToken)
    {
        return await _adapter.GetCoinBalances();
    }
}
