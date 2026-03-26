using Ecosystem.ConfigurationService.Application.Commands.Configuration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class SetWithdrawalsWalletConfigurationHandler : IRequestHandler<SetWithdrawalsWalletConfigurationCommand, WithdrawalsWalletConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<SetWithdrawalsWalletConfigurationHandler> _logger;

    public SetWithdrawalsWalletConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<SetWithdrawalsWalletConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<WithdrawalsWalletConfigurationDto> Handle(SetWithdrawalsWalletConfigurationCommand request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.WithdrawalsWalletConfigurations.MinimumAmount.ToString(),
            ConfigurationEnums.WithdrawalsWalletConfigurations.MaximumAmount.ToString(),
            ConfigurationEnums.WithdrawalsWalletConfigurations.CommissionAmount.ToString(),
            ConfigurationEnums.WithdrawalsWalletConfigurations.ActivateInvoiceCancellation.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);
        var parallelOptions = CommonExtensions.MaxDegreeOfThreads();

        Parallel.ForEach(listConfigurations, parallelOptions, configuration =>
        {
            configuration.Value = configuration.Name switch
            {
                ConfigurationConstants.MinimumAmount => request.MinimumAmount.ToString(),
                ConfigurationConstants.MaximumAmount => request.MaximumAmount.ToString(),
                ConfigurationConstants.CommissionAmount => request.CommissionAmount.ToString(),
                ConfigurationConstants.ActivateInvoiceCancellation => request.ActivateInvoiceCancellation ? "1" : "0",
                _ => configuration.Value
            };
        });

        await _configurationRepository.UpdateConfigurations(listConfigurations);

        return new WithdrawalsWalletConfigurationDto
        {
            MinimumAmount = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.MinimumAmount.ToString())!.Value.ToInt32(),
            MaximumAmount = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.MaximumAmount.ToString())!.Value.ToInt32(),
            CommissionAmount = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.CommissionAmount.ToString())!.Value.ToInt32(),
            ActivateInvoiceCancellation = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.WithdrawalsWalletConfigurations.ActivateInvoiceCancellation.ToString())!.Value.ToBool()
        };
    }
}
