using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class ConceptRepository : BaseRepository, IConceptRepository
{
    public ConceptRepository(ConfigurationServiceDbContext context) : base(context) { }

    public async Task<Concepts> CreateConcept(Concepts concept)
    {
        var today = DateTime.Now;
        concept.CreatedAt = today;
        concept.UpdatedAt = today;

        await Context.Concepts.AddAsync(concept);
        await Context.SaveChangesAsync();

        return concept;
    }

    public Task<Concepts?> GetConceptById(int id)
        => Context.Concepts.Include(i => i.PaymentGroups).FirstOrDefaultAsync(x => x.Id == id);

    public Task<List<Concepts>> GetAllConcepts(long brandId)
        => Context.Concepts.Where(x => x.BrandId == brandId).Include(x => x.PaymentGroups).AsNoTracking().ToListAsync();

    public async Task<Concepts> UpdateConcept(Concepts concept)
    {
        concept.UpdatedAt = DateTime.Now;

        Context.Concepts.Update(concept);
        await Context.SaveChangesAsync();

        return concept;
    }

    public async Task<Concepts> DeleteConcept(Concepts concept)
    {
        var today = DateTime.Now;
        concept.UpdatedAt = today;
        concept.DeletedAt = today;

        Context.Concepts.Update(concept);
        await Context.SaveChangesAsync();

        return concept;
    }
}
