using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class IncentiveRepository : BaseRepository, IIncentiveRepository
{
    public IncentiveRepository(ConfigurationServiceDbContext context) : base(context) { }

    public Task<Incentives?> GetIncentiveById(int id)
        => Context.Incentives.FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<Incentives>> GetAllIncentive(long brandId)
        => Context.Incentives.Where(x => x.BrandId == brandId).AsNoTracking().ToListAsync();

    public async Task<Incentives> CreateIncentive(Incentives incentives)
    {
        var today = DateTime.Now;
        incentives.CreatedAt = today;
        incentives.UpdatedAt = today;

        await Context.Incentives.AddAsync(incentives);
        await Context.SaveChangesAsync();

        return incentives;
    }

    public async Task<Incentives> UpdateIncentive(Incentives incentives)
    {
        incentives.UpdatedAt = DateTime.Now;

        Context.Incentives.Update(incentives);
        await Context.SaveChangesAsync();

        return incentives;
    }

    public async Task<Incentives> DeleteIncentive(Incentives incentives)
    {
        incentives.DeletedAt = DateTime.Now;

        Context.Incentives.Update(incentives);
        await Context.SaveChangesAsync();

        return incentives;
    }
}
