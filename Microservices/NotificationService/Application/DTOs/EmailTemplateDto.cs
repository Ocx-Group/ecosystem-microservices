namespace Ecosystem.NotificationService.Application.DTOs;

public class EmailTemplateDto
{
    public string Id { get; set; } = null!;
    public string TemplateKey { get; set; } = null!;
    public long BrandId { get; set; }
    public string Subject { get; set; } = null!;
    public string HtmlBody { get; set; } = null!;
    public List<string> Placeholders { get; set; } = [];
    public bool IsActive { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
