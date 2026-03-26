using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class GradingRepository : BaseRepository, IGradingRepository
{
    public GradingRepository(ConfigurationServiceDbContext context) : base(context) { }

    public Task<Gradings?> GetGradingById(int id)
        => Context.Gradings.FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<Gradings>> GetAllGrading(long brandId)
        => Context.Gradings.Where(x => x.BrandId == brandId).AsNoTracking().ToListAsync();

    public async Task<Gradings> CreateGrading(Gradings gradings)
    {
        var today = DateTime.Now;
        gradings.CreatedAt = today;
        gradings.UpdatedAt = today;

        await Context.Gradings.AddAsync(gradings);
        await Context.SaveChangesAsync();

        return gradings;
    }

    public async Task<Gradings> UpdateGrading(Gradings gradings)
    {
        gradings.UpdatedAt = DateTime.Now;

        Context.Gradings.Update(gradings);
        await Context.SaveChangesAsync();

        return gradings;
    }

    public async Task<Gradings> DeleteGrading(Gradings gradings)
    {
        gradings.DeletedAt = DateTime.Now;

        Context.Gradings.Update(gradings);
        await Context.SaveChangesAsync();

        return gradings;
    }
}
