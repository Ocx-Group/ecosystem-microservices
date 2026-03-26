using Ecosystem.ConfigurationService.Application.Commands.Configuration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class SetCompensationPlansConfigurationHandler : IRequestHandler<SetCompensationPlansConfigurationCommand, CompensationPlansConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<SetCompensationPlansConfigurationHandler> _logger;

    public SetCompensationPlansConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<SetCompensationPlansConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<CompensationPlansConfigurationDto> Handle(SetCompensationPlansConfigurationCommand request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticActivation.ToString(),
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticQualification.ToString(),
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticIncentiveCalculation.ToString(),
            ConfigurationEnums.CompensationPlansConfigurations.AutomaticCommissionCalculation.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);
        var parallelOptions = CommonExtensions.MaxDegreeOfThreads();

        Parallel.ForEach(listConfigurations, parallelOptions, configuration =>
        {
            configuration.Value = configuration.Name switch
            {
                ConfigurationConstants.AutomaticActivation => request.AutomaticActivation ? "1" : "0",
                ConfigurationConstants.AutomaticQualification => request.AutomaticQualification ? "1" : "0",
                ConfigurationConstants.AutomaticIncentiveCalculation => request.AutomaticIncentiveCalculation ? "1" : "0",
                ConfigurationConstants.AutomaticCommissionCalculation => request.AutomaticCommissionCalculation ? "1" : "0",
                _ => configuration.Value
            };
        });

        await _configurationRepository.UpdateConfigurations(listConfigurations);

        return new CompensationPlansConfigurationDto
        {
            AutomaticActivation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticActivation.ToString())!.Value.ToBool(),
            AutomaticQualification = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticQualification.ToString())!.Value.ToBool(),
            AutomaticIncentiveCalculation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticIncentiveCalculation.ToString())!.Value.ToBool(),
            AutomaticCommissionCalculation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.CompensationPlansConfigurations.AutomaticCommissionCalculation.ToString())!.Value.ToBool()
        };
    }
}
