namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public int CategoryId { get; set; }
    public decimal SalePrice { get; set; }
    public decimal? CommissionableValue { get; set; }
    public decimal? BinaryPoints { get; set; }
    public int? ValuePoints { get; set; }
    public decimal? Tax { get; set; }
    public bool? Inventory { get; set; }
    public int PaymentGroup { get; set; }
    public bool? AcumCompMin { get; set; }
    public decimal? Weight { get; set; }
    public bool? Offer { get; set; }
    public bool? Comment { get; set; }
    public bool? HideCommissionable { get; set; }
    public bool? HidePoint { get; set; }
    public bool? ActiveHtmlContent { get; set; }
    public bool? ActiveZoomPhotos { get; set; }
    public DateTime Date { get; set; }
    public bool? Visible { get; set; }
    public bool? VisiblePublic { get; set; }
    public bool? ProductType { get; set; }
    public bool? State { get; set; }
    public bool? ActivateCombinations { get; set; }
    public bool? ProductPacks { get; set; }
    public decimal? BaseAmount { get; set; }
    public decimal? DailyPercentage { get; set; }
    public int? DaysWait { get; set; }
    public int? AmountDayPay { get; set; }
    public bool? RecurringProduct { get; set; }
    public bool ProductHome { get; set; }
    public int AssociatedQualification { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? DescriptionHtml { get; set; }
    public string? ProductCode { get; set; }
    public string? KeyWord { get; set; }
    public string? Image { get; set; }
    public decimal? ModelTwoPercentage { get; set; }
    public ProductCategoryDto? ProductsCategory { get; set; }
}
