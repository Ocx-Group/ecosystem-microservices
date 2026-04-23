using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class IsUnderMaintenanceHandler : IRequestHandler<IsUnderMaintenanceQuery, bool>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<IsUnderMaintenanceHandler> _logger;

    public IsUnderMaintenanceHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<IsUnderMaintenanceHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<bool> Handle(IsUnderMaintenanceQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var configuration = await _configurationRepository.GetConfigurationByKey(ConfigurationConstants.IsUnderMaintenance, _tenantContext.TenantId);
            if (configuration is null || string.IsNullOrWhiteSpace(configuration.Value)) return false;

            return configuration.Value.ToBool();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resolviendo IsUnderMaintenance para Tenant {TenantId}. Se asume false.", _tenantContext.TenantId);
            return false;
        }
    }
}
