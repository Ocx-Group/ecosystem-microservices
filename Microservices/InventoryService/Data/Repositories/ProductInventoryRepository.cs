using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductInventoryRepository : BaseRepository, IProductInventoryRepository
{
    public ProductInventoryRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<ProductsInventory>> GetProductsInventoryByProductId(int id)
        => Context.ProductsInventories
            .Where(x => x.IdProduct == id)
            .AsNoTracking()
            .ToListAsync();

    public Task<ProductsInventory?> GetProductsInventoryById(int id)
        => Context.ProductsInventories
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProductsInventory> CreateProductsInventoryAsync(ProductsInventory request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.ProductsInventories.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsInventory> UpdateProductInventoryAsync(ProductsInventory request)
    {
        request.UpdatedAt = DateTime.Now;
        Context.ProductsInventories.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }
}
