namespace Ecosystem.NotificationService.Application.DTOs;

public class AttachmentDto
{
    public string FileName { get; set; } = null!;
    public byte[] Content { get; set; } = null!;
    public string ContentType { get; set; } = "application/pdf";
}
