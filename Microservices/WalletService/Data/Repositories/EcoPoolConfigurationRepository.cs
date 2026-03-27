using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Enums;

namespace Ecosystem.WalletService.Data.Repositories;

public class EcoPoolConfigurationRepository : BaseRepository, IEcoPoolConfigurationRepository
{
    public EcoPoolConfigurationRepository(WalletServiceDbContext context) : base(context) { }

    public Task<ModelConfiguration?> GetConfigurationByType(string modelType)
        => Context.ModelConfiguration.Include(x => x.ModelConfigurationLevels).FirstOrDefaultAsync(x => !x.CompletedAt.HasValue && x.ModelType == modelType);

    
    public Task<ModelConfiguration?> GetConfiguration()
        => Context.ModelConfiguration.Include(x => x.ModelConfigurationLevels).FirstOrDefaultAsync(x => !x.CompletedAt.HasValue && x.ModelType == ModelTypeConfiguration.Model_2.ToString());

    public Task<ModelConfiguration> GetProgressPercentage(int configurationId)
    {
        return Context.ModelConfiguration
            .FirstAsync(x => x.Id == configurationId);
    }

    public async Task<ModelConfiguration> CreateConfiguration(ModelConfiguration poolConfiguration)
    {
        poolConfiguration.UpdatedAt = DateTime.Now;
        poolConfiguration.CreatedAt = DateTime.Now;

        await Context.ModelConfiguration.AddAsync(poolConfiguration);
        await Context.SaveChangesAsync();

        return poolConfiguration;
    }

    public Task CreateConfigurationLevels(IEnumerable<ModelConfigurationLevel> levels)
    {
        Context.ModelConfigurationLevels.AddRange(levels);

        return Context.SaveChangesAsync();
    }

    public Task UpdateConfigurationLevels(IEnumerable<ModelConfigurationLevel> levels)
    {
        Context.ModelConfigurationLevels.UpdateRange(levels);

        return Context.SaveChangesAsync();
    }

    public async Task<ModelConfiguration> UpdateConfiguration(ModelConfiguration poolConfiguration)
    {
        poolConfiguration.UpdatedAt = DateTime.Now;

        Context.ModelConfiguration.Update(poolConfiguration);
        await Context.SaveChangesAsync();

        return poolConfiguration;
    }

    public async Task DeleteAllLevelsConfiguration(long configurationId)
    {
        var entities = await Context.ModelConfigurationLevels.Where(x => x.EcopoolConfigurationId == configurationId).ToListAsync();
        if (entities is { Count: > 0 })
            Context.ModelConfigurationLevels.RemoveRange(entities);
    }

}