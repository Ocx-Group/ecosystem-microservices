using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class ConfigurationRepository : BaseRepository, IConfigurationRepository
{
    public ConfigurationRepository(ConfigurationServiceDbContext context) : base(context) { }

    public Task<List<Configurations>> GetConfigurationByKeys(string[] keys, long brandId)
        => Context.Configurations.Where(x => keys.Contains(x.Name) && x.BrandId == brandId).ToListAsync();

    public Task<Configurations?> GetConfigurationByKey(string key, long brandId)
        => Context.Configurations.FirstOrDefaultAsync(x => x.Name == key && x.BrandId == brandId);

    public async Task UpdateConfigurations(IEnumerable<Configurations> configurations)
    {
        Context.Configurations.UpdateRange(configurations);
        await Context.SaveChangesAsync();
    }
}
