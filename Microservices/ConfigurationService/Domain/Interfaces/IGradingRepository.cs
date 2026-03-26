using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IGradingRepository
{
    Task<Gradings?> GetGradingById(int id);
    Task<List<Gradings>> GetAllGrading(long brandId);
    Task<Gradings> CreateGrading(Gradings gradings);
    Task<Gradings> UpdateGrading(Gradings gradings);
    Task<Gradings> DeleteGrading(Gradings gradings);
}
