using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Queries.Configuration;
using Ecosystem.ConfigurationService.Domain.Constants;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Configuration;

public class GetProductConfigurationHandler : IRequestHandler<GetProductConfigurationQuery, ProductConfigurationDto>
{
    private readonly IConfigurationRepository _configurationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetProductConfigurationHandler> _logger;

    public GetProductConfigurationHandler(
        IConfigurationRepository configurationRepository,
        ITenantContext tenantContext,
        ILogger<GetProductConfigurationHandler> logger)
    {
        _configurationRepository = configurationRepository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<ProductConfigurationDto> Handle(GetProductConfigurationQuery request, CancellationToken cancellationToken)
    {
        var keysArray = new[]
        {
            ConfigurationEnums.ProductsConfigurations.ActivateShippingSystem.ToString(),
            ConfigurationEnums.ProductsConfigurations.ActivatePassivePaymentsModule.ToString(),
            ConfigurationEnums.ProductsConfigurations.ActivatePublicShop.ToString(),
            ConfigurationEnums.ProductsConfigurations.CurrencySymbol.ToString(),
            ConfigurationEnums.ProductsConfigurations.SymbolCommissionableValue.ToString(),
            ConfigurationEnums.ProductsConfigurations.SymbolPointsQualify.ToString(),
            ConfigurationEnums.ProductsConfigurations.BinaryPointsSymbol.ToString(),
            ConfigurationEnums.ProductsConfigurations.NewProductLabel.ToString()
        };

        var listConfigurations = await _configurationRepository.GetConfigurationByKeys(keysArray, _tenantContext.TenantId);

        return new ProductConfigurationDto
        {
            ActivateShippingSystem = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.ActivateShippingSystem.ToString())!.Value.ToBool(),
            ActivatePassivePaymentsModule = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.ActivatePassivePaymentsModule.ToString())!.Value.ToBool(),
            ActivatePublicShop = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.ActivatePublicShop.ToString())!.Value.ToBool(),
            CurrencySymbol = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.CurrencySymbol.ToString())?.Value,
            SymbolCommissionableValue = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.SymbolCommissionableValue.ToString())?.Value,
            SymbolPointsQualify = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.SymbolPointsQualify.ToString())?.Value,
            BinaryPointsSymbol = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.BinaryPointsSymbol.ToString())?.Value,
            NewProductLabel = listConfigurations.FirstOrDefault(x => x.Name == ConfigurationEnums.ProductsConfigurations.NewProductLabel.ToString())!.Value.ToInt32()
        };
    }
}
