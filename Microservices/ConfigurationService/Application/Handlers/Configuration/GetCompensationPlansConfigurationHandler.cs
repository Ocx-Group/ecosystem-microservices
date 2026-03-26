using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class GetCompensationPlansConfigurationHandler : IRequestHandler<GetCompensationPlansConfigurationQuery, CompensationPlansConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetCompensationPlansConfigurationHandler> _logger;

    public GetCompensationPlansConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<GetCompensationPlansConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<CompensationPlansConfigurationDto> Handle(GetCompensationPlansConfigurationQuery request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticActivation.ToString(),
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticQualification.ToString(),
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticIncentiveCalculation.ToString(),
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticCommissionCalculation.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);

        return new CompensationPlansConfigurationDto
        {
            AutomaticActivation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticActivation.ToString())!.Value.ToBool(),
            AutomaticQualification = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticQualification.ToString())!.Value.ToBool(),
            AutomaticIncentiveCalculation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticIncentiveCalculation.ToString())!.Value.ToBool(),
            AutomaticCommissionCalculation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticCommissionCalculation.ToString())!.Value.ToBool()
        };
    }
}
