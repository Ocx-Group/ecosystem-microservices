using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class PayMembershipWithBalanceHandler : IRequestHandler<PayMembershipWithBalanceCommand, bool>
{
    private readonly IBalancePaymentStrategy _balancePaymentStrategy;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayMembershipWithBalanceHandler> _logger;

    public PayMembershipWithBalanceHandler(
        IBalancePaymentStrategy balancePaymentStrategy,
        ITenantContext tenantContext,
        ILogger<PayMembershipWithBalanceHandler> logger)
    {
        _balancePaymentStrategy = balancePaymentStrategy;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayMembershipWithBalanceCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (request.ProductsList.Count == 0)
            return false;

        request.BrandId = _tenantContext.TenantId;
        var response = await _balancePaymentStrategy.ExecuteMembershipPayment(request);

        return response;
    }
}
