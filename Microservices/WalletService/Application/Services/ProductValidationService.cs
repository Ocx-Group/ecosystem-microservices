using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Responses;
using Ecosystem.WalletService.Domain.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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
        var response = await _inventoryAdapter.GetProductsIds(productIds, brandId);

        if (!response.IsSuccessful)
        {
            _logger.LogWarning("Inventory service returned unsuccessful response for brand {BrandId}", brandId);
            return ProductValidationResult.Fail("Error al consultar productos en inventario");
        }

        if (string.IsNullOrEmpty(response.Content))
            return ProductValidationResult.Fail("Respuesta vacía del servicio de inventario");

        var result = JsonConvert.DeserializeObject<ProductsResponse>(response.Content);

        if (result?.Data is null || result.Data.Count == 0)
            return ProductValidationResult.Fail("No se encontraron productos");

        if (result.Data.Count != requestedProducts.Count)
        {
            _logger.LogWarning(
                "Product count mismatch: requested {Requested}, found {Found}",
                requestedProducts.Count, result.Data.Count);
            return ProductValidationResult.Fail("Algunos productos no fueron encontrados");
        }

        return ProductValidationResult.Success(result.Data);
    }
}
