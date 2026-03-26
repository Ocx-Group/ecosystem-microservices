using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class GetGeneralConfigurationHandler : IRequestHandler<GetGeneralConfigurationQuery, GeneralConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetGeneralConfigurationHandler> _logger;

    public GetGeneralConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<GetGeneralConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<GeneralConfigurationDto> Handle(GetGeneralConfigurationQuery request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.GeneralConfigurations.PaymentModelCutoffDate.ToString(),
            ConfigurationEnums.GeneralConfigurations.IsUnderMaintenance.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);

        return new GeneralConfigurationDto
        {
            PaymentModelCutoffDate = DateTime.Parse(listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.GeneralConfigurations.PaymentModelCutoffDate.ToString())!.Value),
            IsUnderMaintenance = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.GeneralConfigurations.IsUnderMaintenance.ToString())!.Value.ToBool()
        };
    }
}
