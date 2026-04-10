using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPayments;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class GetDepositAddressHandler : IRequestHandler<GetDepositAddressQuery, GetDepositAddressResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;

    public GetDepositAddressHandler(
        ICoinPaymentsAdapter adapter,
        ILogger<GetDepositAddressHandler> logger)
    {
        _adapter = adapter;
    }

    public async Task<GetDepositAddressResponse?> Handle(GetDepositAddressQuery request, CancellationToken cancellationToken)
    {
        return await _adapter.GetDepositAddress(request.Currency);
    }
}
