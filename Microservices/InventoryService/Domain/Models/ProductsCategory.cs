namespace Ecosystem.InventoryService.Domain.Models;

public partial class ProductsCategory
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int? Category { get; set; }
    public int Level { get; set; }
    public DateTime Date { get; set; }
    public bool? State { get; set; }
    public bool DisplaySmallBanner { get; set; }
    public bool DisplayBigBanner { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long BrandId { get; set; }
    public virtual ICollection<Product> Products { get; } = new List<Product>();
}
