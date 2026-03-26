namespace Ecosystem.InventoryService.Domain.Models;

public partial class ProductsAttribute
{
    public long Id { get; set; }
    public int? Attribute { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool? Type { get; set; }
    public int? Position { get; set; }
    public string? Color { get; set; }
    public bool? State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long BrandId { get; set; }
    public virtual ICollection<ProductsAttributesValue> ProductsAttributesValues { get; } = new List<ProductsAttributesValue>();
    public virtual ICollection<ProductsCombination> ProductsCombinations { get; } = new List<ProductsCombination>();
}
