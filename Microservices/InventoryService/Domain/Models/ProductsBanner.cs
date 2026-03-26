namespace Ecosystem.InventoryService.Domain.Models;

public partial class ProductsBanner
{
    public long Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public bool? ViewInfo { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long BrandId { get; set; }
}
