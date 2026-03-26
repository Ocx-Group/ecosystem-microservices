using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IIncentiveRepository
{
    Task<Incentives?> GetIncentiveById(int id);
    Task<List<Incentives>> GetAllIncentive(long brandId);
    Task<Incentives> CreateIncentive(Incentives incentives);
    Task<Incentives> UpdateIncentive(Incentives incentives);
    Task<Incentives> DeleteIncentive(Incentives incentives);
}
