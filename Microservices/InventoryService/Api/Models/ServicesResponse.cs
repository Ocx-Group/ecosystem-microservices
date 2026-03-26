namespace Ecosystem.InventoryService.Api.Models;

public class ServicesResponse
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string? Message { get; set; }
    public object? Data { get; set; }
}
