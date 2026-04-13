using Ecosystem.Grpc.Inventory;
using Ecosystem.InventoryService.Application.Queries.Product;
using Grpc.Core;
using MediatR;

namespace Ecosystem.InventoryService.Api.GrpcServices;

public class InventoryGrpcService : InventoryGrpc.InventoryGrpcBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<InventoryGrpcService> _logger;

    public InventoryGrpcService(IMediator mediator, ILogger<InventoryGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<GetProductsByIdsResponse> GetProductsByIds(
        GetProductsByIdsRequest request, ServerCallContext context)
    {
        var response = new GetProductsByIdsResponse();

        try
        {
            var ids = request.ProductIds.Select(id => (long)id).ToArray();
            var products = await _mediator.Send(new GetAllProductsByIdsQuery(ids), context.CancellationToken);

            if (products is null || products.Count == 0)
            {
                response.Success = false;
                response.Message = "No products found";
                return response;
            }

            response.Success = true;
            response.Products.AddRange(products.Select(MapToMessage));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting products by IDs");
            response.Success = false;
            response.Message = "Internal error retrieving products";
        }

        return response;
    }

    public override async Task<GetProductByIdResponse> GetProductById(
        GetProductByIdRequest request, ServerCallContext context)
    {
        var response = new GetProductByIdResponse();

        try
        {
            var product = await _mediator.Send(new GetProductByIdQuery(request.ProductId), context.CancellationToken);

            if (product is null)
            {
                response.Success = false;
                response.Message = $"Product {request.ProductId} not found";
                return response;
            }

            response.Success = true;
            response.Product = MapToMessage(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting product {ProductId}", request.ProductId);
            response.Success = false;
            response.Message = "Internal error retrieving product";
        }

        return response;
    }

    public override async Task<UpdateStockResponse> UpdateStock(
        UpdateStockRequest request, ServerCallContext context)
    {
        var response = new UpdateStockResponse();

        try
        {
            var inventoryItems = await _mediator.Send(
                new Application.Queries.ProductInventory.GetProductInventoryByProductIdQuery(request.ProductId),
                context.CancellationToken);

            var current = inventoryItems?.FirstOrDefault();
            if (current is null)
            {
                response.Success = false;
                response.Message = $"No inventory record for product {request.ProductId}";
                return response;
            }

            await _mediator.Send(
                new Application.Commands.ProductInventory.UpdateProductInventoryCommand(
                    Id: current.Id,
                    IdProduct: request.ProductId,
                    Ingress: current.Ingress,
                    Egress: current.Egress + request.Quantity,
                    Support: current.Support ?? string.Empty,
                    Note: $"Stock update via gRPC - qty: {request.Quantity}",
                    Type: (byte)current.Type,
                    Date: DateTime.UtcNow,
                    IdCombination: current.IdCombination ?? 0),
                context.CancellationToken);

            response.Success = true;
            response.Message = "Stock updated successfully";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating stock for product {ProductId}", request.ProductId);
            response.Success = false;
            response.Message = "Internal error updating stock";
        }

        return response;
    }

    private static ProductMessage MapToMessage(Application.DTOs.ProductDto dto) => new()
    {
        Id = dto.Id,
        CategoryId = dto.CategoryId,
        SalePrice = (dto.SalePrice).ToString("G"),
        CommissionableValue = (dto.CommissionableValue ?? 0).ToString("G"),
        BinaryPoints = (dto.BinaryPoints ?? 0).ToString("G"),
        ValuePoints = dto.ValuePoints ?? 0,
        Tax = (dto.Tax ?? 0).ToString("G"),
        PaymentGroup = dto.PaymentGroup,
        AcumCompMin = dto.AcumCompMin ?? false,
        ProductType = dto.ProductType ?? false,
        ProductPacks = dto.ProductPacks ?? false,
        BaseAmount = (dto.BaseAmount ?? 0).ToString("G"),
        DailyPercentage = (dto.DailyPercentage ?? 0).ToString("G"),
        DaysWait = dto.DaysWait ?? 0,
        Name = dto.Name ?? string.Empty,
        ProductDiscount = "0",
        ModelTwoPercentage = (dto.ModelTwoPercentage ?? 0).ToString("G"),
    };
}
