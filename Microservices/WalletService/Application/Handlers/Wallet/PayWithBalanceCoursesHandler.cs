using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class PayWithBalanceCoursesHandler : IRequestHandler<PayWithBalanceCoursesCommand, bool>
{
    private readonly IBalancePaymentStrategy _balancePaymentStrategy;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<PayWithBalanceCoursesHandler> _logger;

    public PayWithBalanceCoursesHandler(
        IBalancePaymentStrategy balancePaymentStrategy,
        ITenantContext tenantContext,
        ILogger<PayWithBalanceCoursesHandler> logger)
    {
        _balancePaymentStrategy = balancePaymentStrategy;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(PayWithBalanceCoursesCommand command, CancellationToken cancellationToken)
    {
        var request = command.Request;
        request.BrandId = _tenantContext.TenantId;
        return await _balancePaymentStrategy.ExecutePaymentCourses(request);
    }
}
