namespace Ecosystem.InventoryService.Application.DTOs;

public class ProductBannerDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public bool? ViewInfo { get; set; }
    public DateTime Date { get; set; }
    public bool Status { get; set; }
}
