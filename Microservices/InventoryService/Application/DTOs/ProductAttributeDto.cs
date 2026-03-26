namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductAttributeDto
{
    public int Id { get; set; }
    public int? IdAttributes { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? Type { get; set; }
    public int? Position { get; set; }
    public string? Color { get; set; }
    public bool? State { get; set; }
}
