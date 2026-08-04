using Ecosystem.WalletService.Application.Queries.CoinPay;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;

namespace Ecosystem.WalletService.Application.Handlers.CoinPay;

public class GetCoinPayTransactionByReferenceHandler : IRequestHandler<GetCoinPayTransactionByReferenceQuery, bool>
{
    private readonly ITransactionRepository _transactionRepository;

    public GetCoinPayTransactionByReferenceHandler(ITransactionRepository transactionRepository)
        => _transactionRepository = transactionRepository;

    public async Task<bool> Handle(GetCoinPayTransactionByReferenceQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Reference))
            return false;

        var transaction = await _transactionRepository.GetTransactionByReference(request.Reference);

        return transaction is { Acredited: true, Status: Constants.CompletedStatusCode };
    }
}
