namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductCombinationDto
{
    public int Id { get; set; }
    public int IdProduct { get; set; }
    public int IdAttributes { get; set; }
    public string? CodeRef { get; set; }
    public bool? DisplayBigBanner { get; set; }
}
