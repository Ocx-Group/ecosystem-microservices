using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductBannerRepository
{
    Task<List<ProductsBanner>> GetAllProductsBanner(long brandId);
    Task<ProductsBanner?> GetProductsBannerById(int id);
    Task<ProductsBanner> CreateProductsBannerAsync(ProductsBanner request);
    Task<ProductsBanner> UpdateProductBannerAsync(ProductsBanner request);
}
