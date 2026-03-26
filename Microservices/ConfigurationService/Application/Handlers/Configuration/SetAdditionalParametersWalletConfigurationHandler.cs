using Ecosystem.ConfigurationService.Application.Commands.Configuration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class SetAdditionalParametersWalletConfigurationHandler : IRequestHandler<SetAdditionalParametersWalletConfigurationCommand, AdditionalParametersConfigurationWalletDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<SetAdditionalParametersWalletConfigurationHandler> _logger;

    public SetAdditionalParametersWalletConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<SetAdditionalParametersWalletConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<AdditionalParametersConfigurationWalletDto> Handle(SetAdditionalParametersWalletConfigurationCommand request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.AdditionalParametersWalletConfigurations.MinutesValidityCode.ToString(),
            ConfigurationEnums.AdditionalParametersWalletConfigurations.ConceptWalletWithdrawal.ToString(),
            ConfigurationEnums.AdditionalParametersWalletConfigurations.ActivateConfirmationMails.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);

        foreach (var configuration in listConfigurations)
        {
            configuration.Value = configuration.Name switch
            {
                ConfigurationConstants.MinutesValidityCode => request.MinutesValidityCode.ToString(),
                ConfigurationConstants.ConceptWalletWithdrawal => request.ConceptWalletWithdrawal,
                ConfigurationConstants.ActivateConfirmationMails => request.ActivateConfirmationMails ? "1" : "0",
                _ => configuration.Value
            };
        }

        await _configurationRepository.UpdateConfigurations(listConfigurations);

        return new AdditionalParametersConfigurationWalletDto
        {
            MinutesValidityCode = Convert.ToInt32(listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.AdditionalParametersWalletConfigurations.MinutesValidityCode.ToString())!.Value),
            ConceptWalletWithdrawal = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.AdditionalParametersWalletConfigurations.ConceptWalletWithdrawal.ToString())!.Value,
            ActivateConfirmationMails = Convert.ToBoolean(Convert.ToInt32(listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.AdditionalParametersWalletConfigurations.ActivateConfirmationMails.ToString())!.Value))
        };
    }
}
