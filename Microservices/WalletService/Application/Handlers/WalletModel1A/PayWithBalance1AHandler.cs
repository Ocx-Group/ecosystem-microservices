using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletModel1A;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletModel1A;

public class PayWithBalance1AHandler : IRequestHandler<PayWithBalance1ACommand, bool>
{
    private readonly IBalancePaymentStrategyModel1A _balancePaymentStrategy;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayWithBalance1AHandler> _logger;

    public PayWithBalance1AHandler(
        IBalancePaymentStrategyModel1A balancePaymentStrategy,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<PayWithBalance1AHandler> logger)
    {
        _balancePaymentStrategy = balancePaymentStrategy;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayWithBalance1ACommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (request.ProductsList.Count == 0)
            return false;

        request.BrandId = _tenantContext.TenantId;
        var response = await _balancePaymentStrategy.ExecuteEcoPoolPayment(request);

        if (response)
            await _cacheService.InvalidateBalanceAsync(request.AffiliateId);

        return response;
    }
}
