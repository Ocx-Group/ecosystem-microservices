namespace Ecosystem.InventoryService.Domain.Models;

public partial class ProductsCombination
{
    public long Id { get; set; }
    public long IdProduct { get; set; }
    public long IdAttributes { get; set; }
    public string CodeRef { get; set; } = null!;
    public bool? DisplayBigBanner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long BrandId { get; set; }
    public virtual ProductsAttribute IdAttributesNavigation { get; set; } = null!;
    public virtual Product IdProductNavigation { get; set; } = null!;
}
