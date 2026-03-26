using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductAttributeValueRepository : BaseRepository, IProductAttributeValueRepository
{
    public ProductAttributeValueRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<ProductsAttributesValue>> GetProductsAttributesValuesByAttributeId(int id)
        => Context.ProductsAttributesValues
            .Where(x => x.IdAttribute == id)
            .AsNoTracking()
            .ToListAsync();

    public Task<ProductsAttributesValue?> GetProductsAttributesValueById(int id)
        => Context.ProductsAttributesValues
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProductsAttributesValue> CreateProductAttributeValueAsync(ProductsAttributesValue request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.ProductsAttributesValues.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsAttributesValue> DeleteProductAttributeValueAsync(ProductsAttributesValue request)
    {
        request.DeletedAt = DateTime.Now;
        Context.ProductsAttributesValues.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }
}
