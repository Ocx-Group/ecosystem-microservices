namespace Ecosystem.NotificationService.Domain.Models;

public class BrandConfiguration
{
    public long Id { get; set; }

    public long BrandId { get; set; }

    public string Name { get; set; } = null!;

    public string SenderName { get; set; } = null!;

    public string SenderEmail { get; set; } = null!;

    public string? SupportEmail { get; set; }

    public string? ClientUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}
