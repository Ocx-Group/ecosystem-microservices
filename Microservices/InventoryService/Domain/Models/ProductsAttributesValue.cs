namespace Ecosystem.InventoryService.Domain.Models;

public partial class ProductsAttributesValue
{
    public long Id { get; set; }
    public long IdAttribute { get; set; }
    public string AttributeValue { get; set; } = null!;
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long BrandId { get; set; }
    public virtual ProductsAttribute IdAttributeNavigation { get; set; } = null!;
}
