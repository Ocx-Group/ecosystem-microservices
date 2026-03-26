namespace Ecosystem.InventoryService.Domain.Models;

public partial class ProductsInventory
{
    public long Id { get; set; }
    public long IdProduct { get; set; }
    public int Ingress { get; set; }
    public int Egress { get; set; }
    public string? Support { get; set; }
    public string? Note { get; set; }
    public DateTime Date { get; set; }
    public int? IdCombination { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public short Type { get; set; }
    public long BrandId { get; set; }
    public virtual Product IdProductNavigation { get; set; } = null!;
}
