using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IConceptConfigurationRepository
{
    Task<ConceptConfigurations> CreateConceptConfiguration(ConceptConfigurations conceptConfigurations);
    Task<ConceptConfigurations> UpdateConceptConfiguration(ConceptConfigurations conceptConfigurations);
    Task<ConceptConfigurations?> GetConceptConfigurationById(int id);
    Task<ConceptConfigurations> DeleteConceptConfiguration(ConceptConfigurations conceptConfigurations);
    Task<List<ConceptConfigurations>> GetAllConceptConfigurationByConceptId(int id, long brandId);
}
