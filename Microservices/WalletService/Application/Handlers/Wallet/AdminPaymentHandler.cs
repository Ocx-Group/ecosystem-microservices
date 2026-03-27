using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class AdminPaymentHandler : IRequestHandler<AdminPaymentCommand, bool>
{
    private readonly IBalancePaymentStrategy _balancePaymentStrategy;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<AdminPaymentHandler> _logger;

    public AdminPaymentHandler(
        IBalancePaymentStrategy balancePaymentStrategy,
        ITenantContext tenantContext,
        ILogger<AdminPaymentHandler> logger)
    {
        _balancePaymentStrategy = balancePaymentStrategy;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(AdminPaymentCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        request.BrandId = _tenantContext.TenantId;
        return await _balancePaymentStrategy.ExecuteAdminPayment(request);
    }
}
