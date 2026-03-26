using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductAttributeValueRepository
{
    Task<List<ProductsAttributesValue>> GetProductsAttributesValuesByAttributeId(int id);
    Task<ProductsAttributesValue?> GetProductsAttributesValueById(int id);
    Task<ProductsAttributesValue> CreateProductAttributeValueAsync(ProductsAttributesValue request);
    Task<ProductsAttributesValue> DeleteProductAttributeValueAsync(ProductsAttributesValue request);
}
