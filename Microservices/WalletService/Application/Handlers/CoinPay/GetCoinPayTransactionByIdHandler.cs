using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPay;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class GetCoinPayTransactionByIdHandler : IRequestHandler<GetCoinPayTransactionByIdQuery, GetTransactionByIdResponse?>
{
    private readonly ICoinPayAdapter _coinPayAdapter;
    private readonly ILogger<GetCoinPayTransactionByIdHandler> _logger;

    public GetCoinPayTransactionByIdHandler(
        ICoinPayAdapter coinPayAdapter,
        ILogger<GetCoinPayTransactionByIdHandler> logger)
    {
        _coinPayAdapter = coinPayAdapter;
        _logger = logger;
    }

    public async Task<GetTransactionByIdResponse?> Handle(GetCoinPayTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        return await _coinPayAdapter.GetTransactionById(request.TransactionId);
    }
}
