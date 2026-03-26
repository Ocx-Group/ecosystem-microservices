using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductBannerRepository : BaseRepository, IProductBannerRepository
{
    public ProductBannerRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<ProductsBanner>> GetAllProductsBanner(long brandId)
        => Context.ProductsBanners
            .Where(x => x.BrandId == brandId)
            .AsNoTracking()
            .ToListAsync();

    public Task<ProductsBanner?> GetProductsBannerById(int id)
        => Context.ProductsBanners
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProductsBanner> CreateProductsBannerAsync(ProductsBanner request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.ProductsBanners.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsBanner> UpdateProductBannerAsync(ProductsBanner request)
    {
        request.UpdatedAt = DateTime.Now;
        Context.ProductsBanners.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }
}
