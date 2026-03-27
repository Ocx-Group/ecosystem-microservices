using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPay;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class GetNetworksByCurrencyHandler : IRequestHandler<GetNetworksByCurrencyQuery, GetNetworkResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly ILogger<GetNetworksByCurrencyHandler> _logger;

    public GetNetworksByCurrencyHandler(
        ICoinPayAdapter coinPayAdapter,
        ILogger<GetNetworksByCurrencyHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _logger = logger;
    }

    public async Task<GetNetworkResponse?> Handle(GetNetworksByCurrencyQuery request, CancellationToken cancellationToken)
    {
        return await _coinPayAdapter.GetNetworksByIdCurrency(request.CurrencyId);
    }
}
