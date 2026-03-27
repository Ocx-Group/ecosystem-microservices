using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Application.Extensions;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.WalletModel1B;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletModel1B;

public class PayWithBalance1BHandler : IRequestHandler<PayWithBalance1BCommand, bool>
{
    private readonly IBalancePaymentStrategyModel1B _balancePaymentStrategy;
    private readonly ICacheService _cacheService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayWithBalance1BHandler> _logger;

    public PayWithBalance1BHandler(
        IBalancePaymentStrategyModel1B balancePaymentStrategy,
        ICacheService cacheService,
        ITenantContext tenantContext,
        ILogger<PayWithBalance1BHandler> logger)
    {
        _balancePaymentStrategy = balancePaymentStrategy;
        _cacheService = cacheService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayWithBalance1BCommand command, CancellationToken cancellationToken)
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
