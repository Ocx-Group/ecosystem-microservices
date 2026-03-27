using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IBrandConfigurationRepository
{
    Task<BrandConfiguration?> GetByBrandIdAsync(long brandId);
    Task<List<BrandConfiguration>> GetAllAsync();
    Task<BrandConfiguration> UpsertAsync(BrandConfiguration config);
    Task<BrandConfiguration?> DeleteAsync(long brandId);
}
