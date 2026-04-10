using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.CoinPayments;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class GetCoinPaymentTransactionInfoHandler : IRequestHandler<GetCoinPaymentTransactionInfoQuery, GetTransactionInfoResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;

    public GetCoinPaymentTransactionInfoHandler(
        ICoinPaymentsAdapter adapter,
        ILogger<GetCoinPaymentTransactionInfoHandler> logger)
    {
        _adapter = adapter;
    }

    public async Task<GetTransactionInfoResponse?> Handle(GetCoinPaymentTransactionInfoQuery request, CancellationToken cancellationToken)
    {
        return await _adapter.GetTransactionInfo(request.TxnId);
    }
}
