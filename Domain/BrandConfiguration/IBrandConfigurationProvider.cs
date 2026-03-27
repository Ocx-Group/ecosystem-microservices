namespace Ecosystem.Domain.Core.BrandConfiguration;

/// <summary>
/// Provides brand configuration to any microservice.
/// ConfigurationService implements this directly against the DB.
/// Other services implement it via HTTP/gRPC adapter with caching.
/// </summary>
public interface IBrandConfigurationProvider
{
    Task<BrandConfigurationDto?> GetByBrandIdAsync(long brandId);
    Task<IReadOnlyList<BrandConfigurationDto>> GetAllAsync();
    Task InvalidateCacheAsync(long? brandId = null);
}
