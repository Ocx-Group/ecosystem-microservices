using Ecosystem.ConfigurationService.Application.Commands.Configuration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class SetGeneralConfigurationHandler : IRequestHandler<SetGeneralConfigurationCommand, GeneralConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<SetGeneralConfigurationHandler> _logger;

    public SetGeneralConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<SetGeneralConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<GeneralConfigurationDto> Handle(SetGeneralConfigurationCommand request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.GeneralConfigurations.PaymentModelCutoffDate.ToString(),
            ConfigurationEnums.GeneralConfigurations.IsUnderMaintenance.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);

        foreach (var configuration in listConfigurations)
        {
            configuration.Value = configuration.Name switch
            {
                ConfigurationConstants.PaymentModelCutoffDate => request.PaymentModelCutoffDate.ToString("o"),
                ConfigurationConstants.IsUnderMaintenance => request.IsUnderMaintenance ? "1" : "0",
                _ => configuration.Value
            };
        }

        await _configurationRepository.UpdateConfigurations(listConfigurations);

        return new GeneralConfigurationDto
        {
            PaymentModelCutoffDate = DateTime.Parse(listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.GeneralConfigurations.PaymentModelCutoffDate.ToString())!.Value),
            IsUnderMaintenance = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.GeneralConfigurations.IsUnderMaintenance.ToString())!.Value.ToBool()
        };
    }
}
