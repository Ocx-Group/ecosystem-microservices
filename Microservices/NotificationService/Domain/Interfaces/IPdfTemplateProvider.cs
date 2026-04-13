namespace Ecosystem.NotificationService.Domain.Interfaces;

/// <summary>
/// Provides PDF HTML templates. Implementation fetches from ConfigurationService.
/// </summary>
public interface IPdfTemplateProvider
{
    Task<PdfTemplate?> GetTemplateAsync(long brandId, string templateKey);
}

public record PdfTemplate(string HtmlContent, string? CssContent, int Version);
