using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IPdfTemplateRepository
{
    Task<PdfTemplate?> GetByBrandAndKeyAsync(long brandId, string templateKey);
    Task<List<PdfTemplate>> GetAllByBrandIdAsync(long brandId);
    Task<PdfTemplate> UpsertAsync(PdfTemplate template);
    Task<PdfTemplate?> DeleteAsync(long brandId, string templateKey);
}
