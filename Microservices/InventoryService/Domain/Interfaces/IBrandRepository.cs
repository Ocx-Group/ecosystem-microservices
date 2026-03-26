using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IBrandRepository
{
    Task<Brand?> GetBrandByIdAsync(string secretKey);
}
