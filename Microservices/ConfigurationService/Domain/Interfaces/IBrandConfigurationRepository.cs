using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IBrandConfigurationRepository
{
    Task<BrandConfiguration?> GetByBrandIdAsync(long brandId);
    Task<List<BrandConfiguration>> GetAllAsync();
    Task<BrandConfiguration> UpsertAsync(BrandConfiguration config);
    Task<BrandConfiguration?> UpdateBrandingAsync(long brandId, BrandConfiguration branding);
    Task<BrandConfiguration?> DeleteAsync(long brandId);
}
