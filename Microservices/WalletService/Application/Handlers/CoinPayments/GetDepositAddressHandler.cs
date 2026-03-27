using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPayments;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class GetDepositAddressHandler : IRequestHandler<GetDepositAddressQuery, GetDepositAddressResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;
    private readonly ILogger<GetDepositAddressHandler> _logger;

    public GetDepositAddressHandler(
        ICoinPaymentsAdapter adapter,
        ILogger<GetDepositAddressHandler> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<GetDepositAddressResponse?> Handle(GetDepositAddressQuery request, CancellationToken cancellationToken)
    {
        return await _adapter.GetDepositAddress(request.Currency);
    }
}
