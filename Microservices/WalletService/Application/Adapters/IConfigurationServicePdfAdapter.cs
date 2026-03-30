namespace Ecosystem.WalletService.Application.Adapters;

public interface IConfigurationServicePdfAdapter
{
    Task<PdfTemplateDto?> GetTemplateAsync(long brandId, string templateKey);
}
