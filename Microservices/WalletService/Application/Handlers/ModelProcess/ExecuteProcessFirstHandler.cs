using Ecosystem.WalletService.Application.Commands.ModelProcess;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.ModelProcess;

public class ExecuteProcessFirstHandler : IRequestHandler<ExecuteProcessFirstCommand, GradingResponse?>
{
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ExecuteProcessFirstHandler> _logger;

    public ExecuteProcessFirstHandler(
        ITenantContext tenantContext,
        ILogger<ExecuteProcessFirstHandler> logger)
    {
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public Task<GradingResponse?> Handle(ExecuteProcessFirstCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ExecuteProcessFirst initiated for brand {BrandId}", _tenantContext.TenantId);
        // Process grading logic will be implemented in the Infrastructure layer
        throw new NotImplementedException("Process grading logic to be implemented in Infrastructure adapter.");
    }
}
