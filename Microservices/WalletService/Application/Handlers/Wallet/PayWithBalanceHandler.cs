using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class PayWithBalanceHandler : IRequestHandler<PayWithBalanceCommand, bool>
{
    private readonly IBalancePaymentStrategy _balancePaymentStrategy;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayWithBalanceHandler> _logger;

    public PayWithBalanceHandler(
        IBalancePaymentStrategy balancePaymentStrategy,
        ITenantContext tenantContext,
        ILogger<PayWithBalanceHandler> logger)
    {
        _balancePaymentStrategy = balancePaymentStrategy;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayWithBalanceCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (request.ProductsList.Count == 0)
            return false;

        request.BrandId = _tenantContext.TenantId;
        var response = await _balancePaymentStrategy.ExecuteEcoPoolPayment(request);

        return response;
    }
}
