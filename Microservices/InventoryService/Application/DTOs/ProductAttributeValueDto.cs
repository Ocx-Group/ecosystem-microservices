namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductAttributeValueDto
{
    public int Id { get; set; }
    public int IdAttribute { get; set; }
    public string? AttributeValue { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
