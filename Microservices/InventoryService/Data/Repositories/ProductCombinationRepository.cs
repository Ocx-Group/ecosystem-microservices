using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ProductCombinationRepository : BaseRepository, IProductCombinationRepository
{
    public ProductCombinationRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<List<ProductsCombination>> GetProductsCombinationsByProductId(int id)
        => Context.ProductsCombinations
            .Where(x => x.IdProduct == id)
            .AsNoTracking()
            .ToListAsync();

    public Task<ProductsCombination?> GetProductsCombinationsById(int id)
        => Context.ProductsCombinations
            .FirstOrDefaultAsync(x => x.Id == id);

    public async Task<ProductsCombination> CreateProductsCombinationAsync(ProductsCombination request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.ProductsCombinations.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<ProductsCombination> DeleteProductCombinationAsync(ProductsCombination request)
    {
        request.DeletedAt = DateTime.Now;
        Context.ProductsCombinations.Update(request);
        await Context.SaveChangesAsync();
        return request;
    }
}
