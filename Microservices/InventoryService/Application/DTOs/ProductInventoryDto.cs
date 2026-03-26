namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductInventoryDto
{
    public int Id { get; set; }
    public int IdProduct { get; set; }
    public int Ingress { get; set; }
    public int Egress { get; set; }
    public string? Support { get; set; }
    public string? Note { get; set; }
    public short Type { get; set; }
    public DateTime Date { get; set; }
    public int? IdCombination { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
