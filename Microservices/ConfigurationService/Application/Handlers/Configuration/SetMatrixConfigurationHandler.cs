using Ecosystem.ConfigurationService.Application.Commands.Configuration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class SetMatrixConfigurationHandler : IRequestHandler<SetMatrixConfigurationCommand, MatrixConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<SetMatrixConfigurationHandler> _logger;

    public SetMatrixConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<SetMatrixConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<MatrixConfigurationDto> Handle(SetMatrixConfigurationCommand request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.MatrixConfigurations.ForceMatrix.ToString(),
            ConfigurationEnums.MatrixConfigurations.UniLevelMatrix.ToString(),
            ConfigurationEnums.MatrixConfigurations.SoftwareMillenniumFrontNum.ToString(),
            ConfigurationEnums.MatrixConfigurations.BinaryMatrix.ToString(),
            ConfigurationEnums.MatrixConfigurations.AffiliatesFrontNum.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);
        var parallelOptions = CommonExtensions.MaxDegreeOfThreads();

        Parallel.ForEach(listConfigurations, parallelOptions, configuration =>
        {
            configuration.Value = configuration.Name switch
            {
                ConfigurationConstants.ForceMatrix => request.ForceMatrix ? "1" : "0",
                ConfigurationConstants.UniLevelMatrix => request.UniLevelMatrix ? "1" : "0",
                ConfigurationConstants.SoftwareMillenniumFrontNum => request.SoftwareMillenniumFrontNum.ToString(),
                ConfigurationConstants.BinaryMatrix => request.BinaryMatrix ? "1" : "0",
                ConfigurationConstants.AffiliatesFrontNum => request.AffiliatesFrontNum.ToString(),
                _ => configuration.Value
            };
        });

        await _configurationRepository.UpdateConfigurations(listConfigurations);

        return new MatrixConfigurationDto
        {
            UniLevelMatrix = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.MatrixConfigurations.UniLevelMatrix.ToString())!.Value.ToBool(),
            ForceMatrix = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.MatrixConfigurations.ForceMatrix.ToString())!.Value.ToBool(),
            BinaryMatrix = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.MatrixConfigurations.BinaryMatrix.ToString())!.Value.ToBool(),
            AffiliatesFrontNum = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.MatrixConfigurations.AffiliatesFrontNum.ToString())!.Value.ToInt32(),
            SoftwareMillenniumFrontNum = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.MatrixConfigurations.SoftwareMillenniumFrontNum.ToString())!.Value.ToInt32()
        };
    }
}
