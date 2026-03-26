using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class GetAdditionalParametersWalletConfigurationHandler : IRequestHandler<GetAdditionalParametersWalletConfigurationQuery, AdditionalParametersConfigurationWalletDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetAdditionalParametersWalletConfigurationHandler> _logger;

    public GetAdditionalParametersWalletConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<GetAdditionalParametersWalletConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<AdditionalParametersConfigurationWalletDto> Handle(GetAdditionalParametersWalletConfigurationQuery request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.AdditionalParametersWalletConfigurations.MinutesValidityCode.ToString(),
            ConfigurationEnums.AdditionalParametersWalletConfigurations.ConceptWalletWithdrawal.ToString(),
            ConfigurationEnums.AdditionalParametersWalletConfigurations.ActivateConfirmationMails.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);

        return new AdditionalParametersConfigurationWalletDto
        {
            MinutesValidityCode = Convert.ToInt32(listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.AdditionalParametersWalletConfigurations.MinutesValidityCode.ToString())!.Value),
            ConceptWalletWithdrawal = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.AdditionalParametersWalletConfigurations.ConceptWalletWithdrawal.ToString())!.Value,
            ActivateConfirmationMails = Convert.ToBoolean(Convert.ToInt32(listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.AdditionalParametersWalletConfigurations.ActivateConfirmationMails.ToString())!.Value))
        };
    }
}
