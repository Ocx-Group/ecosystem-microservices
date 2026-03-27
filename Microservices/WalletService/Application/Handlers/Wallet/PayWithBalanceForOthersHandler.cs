using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class PayWithBalanceForOthersHandler : IRequestHandler<PayWithBalanceForOthersCommand, bool>
{
    private readonly IToThirdPartiesPaymentStrategy _toThirdPartiesPaymentStrategy;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayWithBalanceForOthersHandler> _logger;

    public PayWithBalanceForOthersHandler(
        IToThirdPartiesPaymentStrategy toThirdPartiesPaymentStrategy,
        ITenantContext tenantContext,
        ILogger<PayWithBalanceForOthersHandler> logger)
    {
        _toThirdPartiesPaymentStrategy = toThirdPartiesPaymentStrategy;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayWithBalanceForOthersCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;

        if (request.ProductsList.Count == 0)
            return false;

        request.BrandId = _tenantContext.TenantId;
        var response = await _toThirdPartiesPaymentStrategy.ExecutePayment(request);

        return response;
    }
}
