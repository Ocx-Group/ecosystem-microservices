using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductInventoryRepository
{
    Task<List<ProductsInventory>> GetProductsInventoryByProductId(int id);
    Task<ProductsInventory?> GetProductsInventoryById(int id);
    Task<ProductsInventory> CreateProductsInventoryAsync(ProductsInventory request);
    Task<ProductsInventory> UpdateProductInventoryAsync(ProductsInventory request);
}
