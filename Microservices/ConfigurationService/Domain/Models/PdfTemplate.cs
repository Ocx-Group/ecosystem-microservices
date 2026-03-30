namespace Ecosystem.ConfigurationService.Domain.Models;

/// <summary>
/// HTML/CSS template for PDF generation per brand.
/// Templates use Liquid syntax (Fluid engine) for dynamic content.
/// Placeholders: {{brand.PrimaryColor}}, {{invoice.Total}}, {% for item in invoice.Items %}, etc.
/// </summary>
public class PdfTemplate
{
    public long Id { get; set; }
    public long BrandId { get; set; }
    public string TemplateKey { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
    public string? CssContent { get; set; }
    public bool IsActive { get; set; } = true;
    public int Version { get; set; } = 1;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    // Navigation
    public Brand Brand { get; set; } = null!;
}
