namespace Ecosystem.WalletService.Application.Adapters;

public record PdfTemplateDto(
    string HtmlContent,
    string? CssContent,
    int Version
);
