namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductCategoryDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? IdCategory { get; set; }
    public int Level { get; set; }
    public DateTime Date { get; set; }
    public bool? State { get; set; }
    public bool DisplaySmallBanner { get; set; }
    public bool DisplayBigBanner { get; set; }
}
