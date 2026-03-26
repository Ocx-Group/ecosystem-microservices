using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductAttributeRepository
{
    Task<List<ProductsAttribute>> GetAllProductsAttributes(long brandId);
    Task<ProductsAttribute?> GetProductsAttributesById(int id);
    Task<ProductsAttribute> CreateProductsAttributesAsync(ProductsAttribute request);
    Task<ProductsAttribute> UpdateProductAttributeAsync(ProductsAttribute request);
    Task<ProductsAttribute> DeleteProductAttributeAsync(ProductsAttribute request);
}
