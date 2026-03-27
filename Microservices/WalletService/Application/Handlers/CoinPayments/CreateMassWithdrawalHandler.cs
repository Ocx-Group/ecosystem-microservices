using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.CoinPayments;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.CoinPayments;

public class CreateMassWithdrawalHandler : IRequestHandler<CreateMassWithdrawalCommand, CoinPaymentWithdrawalResponse?>
{
    private readonly ICoinPaymentsAdapter _adapter;
    private readonly ILogger<CreateMassWithdrawalHandler> _logger;

    public CreateMassWithdrawalHandler(
        ICoinPaymentsAdapter adapter,
        ILogger<CreateMassWithdrawalHandler> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<CoinPaymentWithdrawalResponse?> Handle(CreateMassWithdrawalCommand request, CancellationToken cancellationToken)
    {
        return await _adapter.CreateMassWithdrawal(request.Requests);
    }
}
