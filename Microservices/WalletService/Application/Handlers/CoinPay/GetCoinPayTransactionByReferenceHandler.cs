using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPay;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class GetCoinPayTransactionByReferenceHandler : IRequestHandler<GetCoinPayTransactionByReferenceQuery, GetTransactionByIdResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;

    public GetCoinPayTransactionByReferenceHandler(
        ICoinPayAdapter coinPayAdapter,
        ILogger<GetCoinPayTransactionByReferenceHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
    }

    public async Task<GetTransactionByIdResponse?> Handle(GetCoinPayTransactionByReferenceQuery request, CancellationToken cancellationToken)
    {
        return await _coinPayAdapter.GetTransactionByReference(request.Reference);
    }
}
