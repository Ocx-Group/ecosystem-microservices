using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class BrandRepository : BaseRepository, IBrandRepository
{
    public BrandRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<Brand?> GetBrandByIdAsync(string secretKey)
        => Context.Brands.FirstOrDefaultAsync(x => x.SecretKey == secretKey);
}
