using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IConfigurationRepository
{
    Task<List<Configurations>> GetConfigurationByKeys(string[] keys, long brandId);
    Task<Configurations?> GetConfigurationByKey(string key, long brandId);
    Task UpdateConfigurations(IEnumerable<Configurations> configurations);
}
