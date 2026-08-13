using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IBrandConfigurationRepository
{
    Task<BrandConfiguration?> GetByBrandIdAsync(long brandId);
    Task<List<BrandConfiguration>> GetAllAsync();
    Task<BrandConfiguration> UpsertAsync(BrandConfiguration config);
    Task<BrandConfiguration?> UpdateBrandingAsync(long brandId, BrandConfiguration branding);
    Task<BrandConfiguration?> UpdateCommissionSettingsAsync(
        long brandId,
        bool commissionEnabled,
        decimal[] commissionLevels,
        bool dailyBonusAlwaysDistribute);
    Task<BrandConfiguration?> UpdateMonthlyCommissionSettingsAsync(
        long brandId,
        bool enabled,
        decimal interestRate,
        int waitingDays,
        int? paymentGroupId);
    Task<BrandConfiguration?> DeleteAsync(long brandId);
}
