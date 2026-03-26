using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class GetPointsConfigurationHandler : IRequestHandler<GetPointsConfigurationQuery, int?>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetPointsConfigurationHandler> _logger;

    public GetPointsConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<GetPointsConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<int?> Handle(GetPointsConfigurationQuery request, CancellationToken cancellationToken)
    {
        var configuration = await _configurationRepository.GetConfigurationByKey(ConfigurationConstants.PointConfiguration, _tenantContext.TenantId);
        if (configuration is null) return 0;

        return configuration.Value.ToInt32();
    }
}
