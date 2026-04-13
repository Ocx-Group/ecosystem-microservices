using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Services;

public class ProductValidationService : IProductValidationService
{
    private readonly IInventoryServiceAdapter _inventoryAdapter;
    private readonly ILogger<ProductValidationService> _logger;

    public ProductValidationService(
        IInventoryServiceAdapter inventoryAdapter,
        ILogger<ProductValidationService> logger)
    {
        _inventoryAdapter = inventoryAdapter;
        _logger = logger;
    }

    public async Task<ProductValidationResult> ValidateAndGetProducts(
        ICollection<ProductsRequests> requestedProducts,
        long brandId)
    {
        if (requestedProducts is not { Count: > 0 })
            return ProductValidationResult.Fail("La lista de productos está vacía");

        var productIds = requestedProducts.Select(p => p.IdProduct).ToArray();
        var products = await _inventoryAdapter.GetProductsByIdsAsync(productIds, brandId);

        if (products is null || products.Count == 0)
        {
            _logger.LogWarning("Inventory service returned no products for brand {BrandId}", brandId);
            return ProductValidationResult.Fail("No se encontraron productos");
        }

        if (products.Count != requestedProducts.Count)
        {
            _logger.LogWarning(
                "Product count mismatch: requested {Requested}, found {Found}",
                requestedProducts.Count, products.Count);
            return ProductValidationResult.Fail("Algunos productos no fueron encontrados");
        }

        return ProductValidationResult.Success(products);
    }
}
