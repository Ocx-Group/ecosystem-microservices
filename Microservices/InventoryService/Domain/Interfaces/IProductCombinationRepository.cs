using Ecosystem.InventoryService.Domain.Models;

namespace Ecosystem.InventoryService.Domain.Interfaces;

public interface IProductCombinationRepository
{
    Task<List<ProductsCombination>> GetProductsCombinationsByProductId(int id);
    Task<ProductsCombination?> GetProductsCombinationsById(int id);
    Task<ProductsCombination> CreateProductsCombinationAsync(ProductsCombination request);
    Task<ProductsCombination> DeleteProductCombinationAsync(ProductsCombination request);
}
