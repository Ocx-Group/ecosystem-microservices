using Ecosystem.WalletService.Application.Commands.ModelProcess;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.ModelProcess;

public class ExecuteProcessSecondHandler : IRequestHandler<ExecuteProcessSecondCommand, GradingResponse?>
{
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<ExecuteProcessSecondHandler> _logger;

    public ExecuteProcessSecondHandler(
        ITenantContext tenantContext,
        ILogger<ExecuteProcessSecondHandler> logger)
    {
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public Task<GradingResponse?> Handle(ExecuteProcessSecondCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("ExecuteProcessSecond initiated for brand {BrandId}", _tenantContext.TenantId);
        // Process grading logic will be implemented in the Infrastructure layer
        throw new NotImplementedException("Process grading logic to be implemented in Infrastructure adapter.");
    }
}
