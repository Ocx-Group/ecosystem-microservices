using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class ConceptConfigurationRepository : BaseRepository, IConceptConfigurationRepository
{
    public ConceptConfigurationRepository(ConfigurationServiceDbContext context) : base(context) { }

    public async Task<ConceptConfigurations> CreateConceptConfiguration(ConceptConfigurations conceptConfigurations)
    {
        var today = DateTime.Now;
        conceptConfigurations.CreatedAt = today;
        conceptConfigurations.UpdatedAt = today;

        await Context.ConceptConfigurations.AddAsync(conceptConfigurations);
        await Context.SaveChangesAsync();

        return conceptConfigurations;
    }

    public async Task<ConceptConfigurations> UpdateConceptConfiguration(ConceptConfigurations conceptConfigurations)
    {
        conceptConfigurations.UpdatedAt = DateTime.Now;

        Context.ConceptConfigurations.Update(conceptConfigurations);
        await Context.SaveChangesAsync();

        return conceptConfigurations;
    }

    public Task<ConceptConfigurations?> GetConceptConfigurationById(int id)
        => Context.ConceptConfigurations.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ConceptConfigurations> DeleteConceptConfiguration(ConceptConfigurations conceptConfigurations)
    {
        var today = DateTime.Now;
        conceptConfigurations.DeletedAt = today;
        conceptConfigurations.UpdatedAt = today;

        Context.ConceptConfigurations.Update(conceptConfigurations);
        await Context.SaveChangesAsync();

        return conceptConfigurations;
    }

    public Task<List<ConceptConfigurations>> GetAllConceptConfigurationByConceptId(int id, long brandId)
        => Context.ConceptConfigurations.Where(x => x.ConceptId == id && x.BrandId == brandId).ToListAsync();
}
