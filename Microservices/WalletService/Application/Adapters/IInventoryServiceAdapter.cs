using Ecosystem.WalletService.Domain.Responses.BaseResponses;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Replace HTTP calls with gRPC/RabbitMQ inter-service communication
public interface IInventoryServiceAdapter
{
    Task<IRestResponse> GetProductsIds(int[] productIds, long brandId);
    Task<IRestResponse> GetProductById(int productId, long brandId);
    Task<RestResponse> UpdateStock(int productId, int quantity, long brandId);
}
