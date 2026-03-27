using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class PayWithBalanceModel2Handler : IRequestHandler<PayWithBalanceModel2Command, bool>
{
    private readonly IBalancePaymentStrategyModel2 _balancePaymentStrategyModel2;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayWithBalanceModel2Handler> _logger;

    public PayWithBalanceModel2Handler(
        IBalancePaymentStrategyModel2 balancePaymentStrategyModel2,
        ITenantContext tenantContext,
        ILogger<PayWithBalanceModel2Handler> logger)
    {
        _balancePaymentStrategyModel2 = balancePaymentStrategyModel2;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayWithBalanceModel2Command command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (request.ProductsList.Count == 0)
            return false;

        request.BrandId = _tenantContext.TenantId;
        var response = await _balancePaymentStrategyModel2.ExecutePayment(request);

        return response;
    }
}
