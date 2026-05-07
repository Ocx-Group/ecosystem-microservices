using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.NotificationService.Domain.Interfaces;

namespace Ecosystem.NotificationService.Data.Repositories;

public class BrandTenantStore : ITenantStore
{
    private readonly IBrandRepository _brandRepository;

    public BrandTenantStore(IBrandRepository brandRepository)
        => _brandRepository = brandRepository;

    public async Task<TenantInfo?> ResolveTenantAsync(string secretKey)
    {
        var brand = await _brandRepository.GetBrandBySecretKeyAsync(secretKey);
        return brand is null ? null : new TenantInfo(brand.Id, brand.Name);
    }
}
