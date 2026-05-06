using Ecosystem.NotificationService.Domain.Models;

namespace Ecosystem.NotificationService.Domain.Interfaces;

public interface IBrandConfigurationRepository
{
    Task<BrandConfiguration?> GetByBrandIdAsync(long brandId);
    Task<ICollection<BrandConfiguration>> GetAllAsync();
    Task<BrandConfiguration?> GetByIdAsync(long id);
    Task<BrandConfiguration> CreateAsync(BrandConfiguration brand);
    Task<BrandConfiguration> UpdateAsync(BrandConfiguration brand);
    Task<bool> DeleteAsync(long id);
}
