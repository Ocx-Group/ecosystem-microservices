namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Replace HTTP calls with gRPC/RabbitMQ inter-service communication
public interface IConfigurationServiceAdapter
{
    Task<RestResponse> GetProductById(int productId, long brandId);
    Task<RestResponse> GetProductsByIds(int[] productIds, long brandId);
}
