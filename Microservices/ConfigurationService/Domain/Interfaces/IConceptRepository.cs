using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IConceptRepository
{
    Task<Concepts> CreateConcept(Concepts concept);
    Task<Concepts?> GetConceptById(int id);
    Task<List<Concepts>> GetAllConcepts(long brandId);
    Task<Concepts> UpdateConcept(Concepts concept);
    Task<Concepts> DeleteConcept(Concepts concept);
}
