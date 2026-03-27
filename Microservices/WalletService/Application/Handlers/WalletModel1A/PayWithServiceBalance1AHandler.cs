using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletModel1A;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletModel1A;

public class PayWithServiceBalance1AHandler : IRequestHandler<PayWithServiceBalance1ACommand, bool>
{
    private readonly IBalancePaymentStrategyModel1A _balancePaymentStrategy;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayWithServiceBalance1AHandler> _logger;

    public PayWithServiceBalance1AHandler(
        IBalancePaymentStrategyModel1A balancePaymentStrategy,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<PayWithServiceBalance1AHandler> logger)
    {
        _balancePaymentStrategy = balancePaymentStrategy;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayWithServiceBalance1ACommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (request.ProductsList.Count == 0)
            return false;

        request.BrandId = _tenantContext.TenantId;
        var response = await _balancePaymentStrategy.ExecuteEcoPoolPaymentWithServiceBalance(request);

        if (response)
            await _cacheService.InvalidateBalanceAsync(request.AffiliateId);

        return response;
    }
}
