using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class GetMatrixConfigurationHandler : IRequestHandler<GetMatrixConfigurationQuery, MatrixConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetMatrixConfigurationHandler> _logger;

    public GetMatrixConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<GetMatrixConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<MatrixConfigurationDto> Handle(GetMatrixConfigurationQuery request, CancellationToken cancellationToken)
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
