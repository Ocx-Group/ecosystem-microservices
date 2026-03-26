using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductCategoryRepository : BaseRepository, IProductCategoryRepository
{
    public ProductCategoryRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<ProductsCategory>> GetAllProductsCategories(long brandId)
        => Context.ProductsCategories
            .Where(x => x.BrandId == brandId)
            .AsNoTracking()
            .ToListAsync();

    public Task<ProductsCategory?> GetProductsCategoriesById(int id)
        => Context.ProductsCategories
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProductsCategory> CreateProductsCategoryAsync(ProductsCategory request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.ProductsCategories.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsCategory> UpdateProductCategoryAsync(ProductsCategory request)
    {
        request.UpdatedAt = DateTime.Now;
        Context.ProductsCategories.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsCategory> DeleteProductCategoryAsync(ProductsCategory request)
    {
        request.DeletedAt = DateTime.Now;
        Context.ProductsCategories.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }
}
