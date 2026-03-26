namespace Ecosystem.InventoryService.Domain.Models;

public partial class ProductsDiscount
{
    public long Id { get; set; }
    public long IdProduct { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public int Qualification { get; set; }
    public decimal Percentage { get; set; }
    public bool? PointsQualify { get; set; }
    public bool? BinaryPoints { get; set; }
    public bool? Commissionable { get; set; }
    public decimal PCommissionable { get; set; }
    public decimal? PBinaryPoints { get; set; }
    public decimal PPointsQualify { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long BrandId { get; set; }
    public virtual Product IdProductNavigation { get; set; } = null!;
}
