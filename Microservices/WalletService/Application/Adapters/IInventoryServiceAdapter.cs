using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;

namespace Ecosystem.WalletService.Application.Adapters;

public interface IInventoryServiceAdapter
{
    Task<List<ProductWalletDto>?> GetProductsByIdsAsync(int[] productIds, long brandId);
    Task<ProductWalletDto?> GetProductByIdAsync(int productId, long brandId);
    Task<bool> UpdateStockAsync(int productId, int quantity, long brandId);
}
