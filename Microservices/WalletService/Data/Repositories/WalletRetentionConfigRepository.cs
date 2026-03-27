using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class WalletRetentionConfigRepository : BaseRepository, IWalletRetentionConfigRepository
{
    public WalletRetentionConfigRepository(WalletServiceDbContext context) : base(context) { }

    public Task<List<WalletsRetentionsConfig>> GetAllWalletsRetentionConfig()
        => Context.WalletsRetentionsConfigs.AsNoTracking().ToListAsync();

    public Task<WalletsRetentionsConfig?> GetWalletRetentionConfigById(int id)
        => Context.WalletsRetentionsConfigs.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<WalletsRetentionsConfig>> CreateWalletRetentionConfigAsync(IEnumerable<WalletsRetentionsConfig> request)
    {
        var today                  = DateTime.Now;
        var createRetentionConfigs = new List<WalletsRetentionsConfig>();

        foreach (var periodRetention in request)
        {
            periodRetention.CreatedAt = today;
            periodRetention.UpdatedAt = today;

            await Context.AddAsync(periodRetention);
            createRetentionConfigs.Add(periodRetention);
        }

        await Context.SaveChangesAsync();

        return createRetentionConfigs;
    }

    public async Task<IEnumerable<WalletsRetentionsConfig>> UpdateWalletRetentionConfigAsync(IEnumerable<WalletsRetentionsConfig> request)
    {
        var today                   = DateTime.Now;
        var updatedRetentionConfigs = new List<WalletsRetentionsConfig>();

        foreach (var periodRetention in request)
        {
            periodRetention.CreatedAt = today;
            periodRetention.UpdatedAt = today;

            Context.WalletsRetentionsConfigs.Update(periodRetention);
            updatedRetentionConfigs.Add(periodRetention);
        }

        await Context.SaveChangesAsync();

        return updatedRetentionConfigs;
    }


    public async Task<WalletsRetentionsConfig> DeleteWalletRetentionConfigAsync(WalletsRetentionsConfig request)
    {
        request.DeletedAt = DateTime.Now;

        Context.WalletsRetentionsConfigs.Update(request);
        await Context.SaveChangesAsync();

        return request;
    }
}