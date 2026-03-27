using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPayments;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class GetCoinPaymentsProfileHandler : IRequestHandler<GetCoinPaymentsProfileQuery, GetBasicInfoResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;
    private readonly ILogger<GetCoinPaymentsProfileHandler> _logger;

    public GetCoinPaymentsProfileHandler(
        ICoinPaymentsAdapter adapter,
        ILogger<GetCoinPaymentsProfileHandler> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<GetBasicInfoResponse?> Handle(GetCoinPaymentsProfileQuery request, CancellationToken cancellationToken)
    {
        return await _adapter.GetProfile();
    }
}
