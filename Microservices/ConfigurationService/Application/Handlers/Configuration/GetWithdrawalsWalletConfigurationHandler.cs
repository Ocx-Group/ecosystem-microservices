using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class GetWithdrawalsWalletConfigurationHandler : IRequestHandler<GetWithdrawalsWalletConfigurationQuery, WithdrawalsWalletConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetWithdrawalsWalletConfigurationHandler> _logger;

    public GetWithdrawalsWalletConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<GetWithdrawalsWalletConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<WithdrawalsWalletConfigurationDto> Handle(GetWithdrawalsWalletConfigurationQuery request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.WithdrawalsWalletConfigurations.MinimumAmount.ToString(),
            ConfigurationEnums.WithdrawalsWalletConfigurations.MaximumAmount.ToString(),
            ConfigurationEnums.WithdrawalsWalletConfigurations.CommissionAmount.ToString(),
            ConfigurationEnums.WithdrawalsWalletConfigurations.ActivateInvoiceCancellation.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);

        return new WithdrawalsWalletConfigurationDto
        {
            MinimumAmount = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.MinimumAmount.ToString())!.Value.ToInt32(),
            MaximumAmount = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.MaximumAmount.ToString())!.Value.ToInt32(),
            CommissionAmount = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.CommissionAmount.ToString())!.Value.ToInt32(),
            ActivateInvoiceCancellation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.ActivateInvoiceCancellation.ToString())!.Value.ToBool()
        };
    }
}
