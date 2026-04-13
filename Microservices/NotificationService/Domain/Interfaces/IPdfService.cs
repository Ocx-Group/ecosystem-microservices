namespace Ecosystem.NotificationService.Domain.Interfaces;

public interface IPdfService
{
    /// <summary>
    /// Generates a PDF from an HTML template, hydrated with brand config + data.
    /// </summary>
    Task<byte[]> GenerateFromTemplateAsync(string templateKey, long brandId, object data);
}
