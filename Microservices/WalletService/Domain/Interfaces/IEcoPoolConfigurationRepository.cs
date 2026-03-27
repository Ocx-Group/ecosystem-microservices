using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IEcoPoolConfigurationRepository
{
    Task<ModelConfiguration?> GetConfigurationByType(string modelType);
    Task<ModelConfiguration?> GetConfiguration();
    Task<ModelConfiguration> CreateConfiguration(ModelConfiguration poolConfiguration);
    Task CreateConfigurationLevels(IEnumerable<ModelConfigurationLevel>           levels);
    Task<ModelConfiguration> UpdateConfiguration(ModelConfiguration poolConfiguration);
    Task DeleteAllLevelsConfiguration(long                               configurationId);
    Task<ModelConfiguration> GetProgressPercentage(int                configurationId);
}