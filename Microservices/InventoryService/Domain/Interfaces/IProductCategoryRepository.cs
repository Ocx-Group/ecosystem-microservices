using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductCategoryRepository
{
    Task<List<ProductsCategory>> GetAllProductsCategories(long brandId);
    Task<ProductsCategory?> GetProductsCategoriesById(int id);
    Task<ProductsCategory> CreateProductsCategoryAsync(ProductsCategory request);
    Task<ProductsCategory> UpdateProductCategoryAsync(ProductsCategory request);
    Task<ProductsCategory> DeleteProductCategoryAsync(ProductsCategory request);
}
