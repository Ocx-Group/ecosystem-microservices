using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductDiscountRepository : BaseRepository, IProductDiscountRepository
{
    public ProductDiscountRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<ProductsDiscount>> GetAllProductsDiscounts(long brandId)
        => Context.ProductsDiscounts
            .Where(x => x.BrandId == brandId)
            .AsNoTracking()
            .ToListAsync();

    public Task<List<ProductsDiscount>> GetAllProductsDiscountsByProductId(int id)
        => Context.ProductsDiscounts
            .Where(x => x.IdProduct == id)
            .AsNoTracking()
            .ToListAsync();

    public Task<ProductsDiscount?> GetProductsDiscountById(int id)
        => Context.ProductsDiscounts
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProductsDiscount> CreateProductsDiscountAsync(ProductsDiscount request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.ProductsDiscounts.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsDiscount> UpdateProductDiscountAsync(ProductsDiscount request)
    {
        request.UpdatedAt = DateTime.Now;
        Context.ProductsDiscounts.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsDiscount> DeleteProductDiscountAsync(ProductsDiscount request)
    {
        request.DeletedAt = DateTime.Now;
        Context.ProductsDiscounts.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }
}
