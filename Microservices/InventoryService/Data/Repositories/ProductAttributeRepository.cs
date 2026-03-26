using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductAttributeRepository : BaseRepository, IProductAttributeRepository
{
    public ProductAttributeRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<ProductsAttribute>> GetAllProductsAttributes(long brandId)
        => Context.ProductsAttributes
            .Where(x => x.BrandId == brandId)
            .AsNoTracking()
            .ToListAsync();

    public Task<ProductsAttribute?> GetProductsAttributesById(int id)
        => Context.ProductsAttributes
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProductsAttribute> CreateProductsAttributesAsync(ProductsAttribute request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.ProductsAttributes.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsAttribute> UpdateProductAttributeAsync(ProductsAttribute request)
    {
        request.UpdatedAt = DateTime.Now;
        Context.ProductsAttributes.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsAttribute> DeleteProductAttributeAsync(ProductsAttribute request)
    {
        request.DeletedAt = DateTime.Now;
        Context.ProductsAttributes.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }
}
