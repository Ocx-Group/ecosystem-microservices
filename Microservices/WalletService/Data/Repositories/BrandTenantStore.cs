using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.WalletService.Data.Repositories;

public class BrandTenantStore : ITenantStore
{
    private readonly WalletServiceDbContext _context;

    public BrandTenantStore(WalletServiceDbContext context)
        => _context = context;

    public async Task<TenantInfo?> ResolveTenantAsync(string identifier)
    {
        var brand = await _context.Brand
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.SecretKey == identifier);

        return brand is null
            ? null
            : new TenantInfo(brand.Id, brand.Name ?? string.Empty);
    }
}
