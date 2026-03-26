using Ecosystem.InventoryService.Application.DTOs;
using MediatR;

namespace Ecosystem.InventoryService.Application.Commands.Product;

public record CreateProductCommand : IRequest<ProductDto?>
{
    public long CategoryId { get; init; }
    public decimal SalePrice { get; init; }
    public decimal? CommissionableValue { get; init; }
    public decimal? BinaryPoints { get; init; }
    public int? ValuePoints { get; init; }
    public decimal? Tax { get; init; }
    public bool? Inventory { get; init; }
    public int PaymentGroup { get; init; }
    public bool? AcumCompMin { get; init; }
    public decimal? Weight { get; init; }
    public bool? Offer { get; init; }
    public bool? Comment { get; init; }
    public bool? HideCommissionable { get; init; }
    public bool? HidePoint { get; init; }
    public bool? ActiveHtmlContent { get; init; }
    public bool? ActiveZoomPhotos { get; init; }
    public bool? Visible { get; init; }
    public bool? VisiblePublic { get; init; }
    public bool? ProductType { get; init; }
    public bool? State { get; init; }
    public bool? ActivateCombinations { get; init; }
    public bool? ProductPacks { get; init; }
    public decimal? BaseAmount { get; init; }
    public decimal? DailyPercentage { get; init; }
    public int? DaysWait { get; init; }
    public int? AmountDayPay { get; init; }
    public bool? RecurringProduct { get; init; }
    public bool ProductHome { get; init; }
    public int AssociatedQualification { get; init; }
    public string Name { get; init; } = null!;
    public string? Description { get; init; }
    public string? DescriptionHtml { get; init; }
    public string? ProductCode { get; init; }
    public string? Keyword { get; init; }
    public string? Image { get; init; }
    public decimal? ModelTwoPercentage { get; init; }
}
