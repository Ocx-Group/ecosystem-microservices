namespace Ecosystem.InventoryService.Application.Adapters;

public interface IConfigurationServiceAdapter
{
    Task<InventoryBrandConfiguration?> GetBrandConfigurationAsync(
        long brandId,
        CancellationToken cancellationToken = default);
}

public sealed record InventoryBrandConfiguration
{
    public long BrandId { get; init; }
    public int? DefaultPaymentGroupId { get; init; }
    public int? TradingAcademyPaymentGroupId { get; init; }
}
