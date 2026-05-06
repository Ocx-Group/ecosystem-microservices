using Ecosystem.NotificationService.Data.Context;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.NotificationService.Data.Repositories;

public class BrandConfigurationRepository : IBrandConfigurationRepository
{
    private readonly NotificationServiceDbContext _context;

    public BrandConfigurationRepository(NotificationServiceDbContext context)
        => _context = context;

    public async Task<BrandConfiguration?> GetByBrandIdAsync(long brandId)
        => await _context.BrandConfigurations
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.BrandId == brandId && b.IsActive);

    public async Task<ICollection<BrandConfiguration>> GetAllAsync()
        => await _context.BrandConfigurations
            .AsNoTracking()
            .OrderBy(b => b.BrandId)
            .ToListAsync();

    public async Task<BrandConfiguration?> GetByIdAsync(long id)
        => await _context.BrandConfigurations.FindAsync(id);

    public async Task<BrandConfiguration> CreateAsync(BrandConfiguration brand)
    {
        brand.CreatedAt = DateTime.UtcNow;
        _context.BrandConfigurations.Add(brand);
        await _context.SaveChangesAsync();
        return brand;
    }

    public async Task<BrandConfiguration> UpdateAsync(BrandConfiguration brand)
    {
        brand.UpdatedAt = DateTime.UtcNow;
        _context.BrandConfigurations.Update(brand);
        await _context.SaveChangesAsync();
        return brand;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var brand = await _context.BrandConfigurations.FindAsync(id);
        if (brand is null) return false;
        _context.BrandConfigurations.Remove(brand);
        await _context.SaveChangesAsync();
        return true;
    }
}
