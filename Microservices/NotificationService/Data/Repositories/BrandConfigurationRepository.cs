using Ecosystem.NotificationService.Data.Context;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using MongoDB.Driver;

namespace Ecosystem.NotificationService.Data.Repositories;

public class BrandConfigurationRepository : IBrandConfigurationRepository
{
    private readonly MongoDbContext _context;

    public BrandConfigurationRepository(MongoDbContext context)
        => _context = context;

    public async Task<BrandConfiguration?> GetByBrandIdAsync(long brandId)
        => await _context.BrandConfigurations
            .Find(b => b.BrandId == brandId && b.IsActive)
            .FirstOrDefaultAsync();

    public async Task<ICollection<BrandConfiguration>> GetAllAsync()
        => await _context.BrandConfigurations
            .Find(_ => true)
            .SortBy(b => b.BrandId)
            .ToListAsync();

    public async Task<BrandConfiguration?> GetByIdAsync(string id)
        => await _context.BrandConfigurations
            .Find(b => b.Id == id)
            .FirstOrDefaultAsync();

    public async Task<BrandConfiguration> CreateAsync(BrandConfiguration brand)
    {
        brand.CreatedAt = DateTime.UtcNow;
        await _context.BrandConfigurations.InsertOneAsync(brand);
        return brand;
    }

    public async Task<BrandConfiguration> UpdateAsync(BrandConfiguration brand)
    {
        brand.UpdatedAt = DateTime.UtcNow;
        await _context.BrandConfigurations.ReplaceOneAsync(b => b.Id == brand.Id, brand);
        return brand;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _context.BrandConfigurations.DeleteOneAsync(b => b.Id == id);
        return result.DeletedCount > 0;
    }
}
