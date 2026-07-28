using Ecosystem.Grpc.Configuration;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Adapters;

public sealed class GrpcConfigurationServiceAdapter : IConfigurationServiceAdapter
{
    private readonly ConfigurationGrpc.ConfigurationGrpcClient _client;
    private readonly ILogger<GrpcConfigurationServiceAdapter> _logger;

    public GrpcConfigurationServiceAdapter(
        ConfigurationGrpc.ConfigurationGrpcClient client,
        ILogger<GrpcConfigurationServiceAdapter> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<InventoryBrandConfiguration?> GetBrandConfigurationAsync(
        long brandId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.GetBrandConfigurationAsync(
                new GetBrandConfigurationRequest { BrandId = brandId },
                deadline: DateTime.UtcNow.AddSeconds(10),
                cancellationToken: cancellationToken);

            if (!response.Success || response.Configuration is null)
            {
                _logger.LogWarning(
                    "Brand configuration not found for brand {BrandId}: {Message}",
                    brandId,
                    response.Message);
                return null;
            }

            var source = response.Configuration;
            return new InventoryBrandConfiguration
            {
                BrandId = source.BrandId,
                DefaultPaymentGroupId = source.HasDefaultPaymentGroupId
                    ? source.DefaultPaymentGroupId
                    : null,
                TradingAcademyPaymentGroupId = source.HasTradingAcademyPaymentGroupId
                    ? source.TradingAcademyPaymentGroupId
                    : null
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "gRPC error retrieving brand configuration for brand {BrandId}",
                brandId);
            throw;
        }
    }
}
