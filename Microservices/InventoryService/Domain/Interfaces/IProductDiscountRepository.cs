using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductDiscountRepository
{
    Task<List<ProductsDiscount>> GetAllProductsDiscounts(long brandId);
    Task<List<ProductsDiscount>> GetAllProductsDiscountsByProductId(int id);
    Task<ProductsDiscount?> GetProductsDiscountById(int id);
    Task<ProductsDiscount> CreateProductsDiscountAsync(ProductsDiscount request);
    Task<ProductsDiscount> UpdateProductDiscountAsync(ProductsDiscount request);
    Task<ProductsDiscount> DeleteProductDiscountAsync(ProductsDiscount request);
}
