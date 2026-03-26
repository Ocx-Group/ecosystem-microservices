namespace Ecosystem.NotificationService.Application.DTOs;

public class BrandConfigurationDto
{
    public string Id { get; set; } = null!;
    public long BrandId { get; set; }
    public string Name { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
    public string? SupportEmail { get; set; }
    public string? ClientUrl { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
