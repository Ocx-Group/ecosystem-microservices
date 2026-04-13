using AutoMapper;
using Ecosystem.Grpc.Inventory;
using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Adapters;

public class GrpcInventoryServiceAdapter : IInventoryServiceAdapter
{
    private readonly InventoryGrpc.InventoryGrpcClient _client;
    private readonly ILogger<GrpcInventoryServiceAdapter> _logger;
    private readonly IMapper _mapper;

    public GrpcInventoryServiceAdapter(
        InventoryGrpc.InventoryGrpcClient client,
        ILogger<GrpcInventoryServiceAdapter> logger,
        IMapper mapper)
    {
        _client = client;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<List<ProductWalletDto>?> GetProductsByIdsAsync(int[] productIds, long brandId)
    {
        var request = new GetProductsByIdsRequest { BrandId = brandId };
        request.ProductIds.AddRange(productIds);

        var response = await _client.GetProductsByIdsAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("gRPC GetProductsByIds failed: {Message}", response.Message);
            return null;
        }

        return response.Products.Select(p => _mapper.Map<ProductWalletDto>(p)).ToList();
    }

    public async Task<ProductWalletDto?> GetProductByIdAsync(int productId, long brandId)
    {
        var request = new GetProductByIdRequest { ProductId = productId, BrandId = brandId };
        var response = await _client.GetProductByIdAsync(request);

        if (!response.Success)
        {
            _logger.LogWarning("gRPC GetProductById failed for {ProductId}: {Message}", productId, response.Message);
            return null;
        }

        return response.Product is not null ? _mapper.Map<ProductWalletDto>(response.Product) : null;
    }

    public async Task<bool> UpdateStockAsync(int productId, int quantity, long brandId)
    {
        var request = new UpdateStockRequest
        {
            ProductId = productId,
            Quantity = quantity,
            BrandId = brandId
        };

        var response = await _client.UpdateStockAsync(request);

        if (!response.Success)
            _logger.LogWarning("gRPC UpdateStock failed for product {ProductId}: {Message}", productId, response.Message);

        return response.Success;
    }
}


