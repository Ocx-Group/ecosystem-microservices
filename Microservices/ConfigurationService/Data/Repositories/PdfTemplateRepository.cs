using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class PdfTemplateRepository : BaseRepository, IPdfTemplateRepository
{
    public PdfTemplateRepository(ConfigurationServiceDbContext context) : base(context) { }

    public Task<PdfTemplate?> GetByBrandAndKeyAsync(long brandId, string templateKey)
        => Context.PdfTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.BrandId == brandId && x.TemplateKey == templateKey);

    public Task<List<PdfTemplate>> GetAllByBrandIdAsync(long brandId)
        => Context.PdfTemplates
            .AsNoTracking()
            .Where(x => x.BrandId == brandId)
            .ToListAsync();

    public async Task<PdfTemplate> UpsertAsync(PdfTemplate template)
    {
        var existing = await Context.PdfTemplates
            .FirstOrDefaultAsync(x => x.BrandId == template.BrandId && x.TemplateKey == template.TemplateKey);

        var now = DateTime.UtcNow;

        if (existing is null)
        {
            template.CreatedAt = now;
            template.UpdatedAt = now;
            await Context.PdfTemplates.AddAsync(template);
        }
        else
        {
            existing.HtmlContent = template.HtmlContent;
            existing.CssContent = template.CssContent;
            existing.IsActive = template.IsActive;
            existing.Version += 1;
            existing.UpdatedAt = now;
            template = existing;
        }

        await Context.SaveChangesAsync();
        return template;
    }

    public async Task<PdfTemplate?> DeleteAsync(long brandId, string templateKey)
    {
        var existing = await Context.PdfTemplates
            .FirstOrDefaultAsync(x => x.BrandId == brandId && x.TemplateKey == templateKey);

        if (existing is null) return null;

        existing.DeletedAt = DateTime.UtcNow;
        existing.UpdatedAt = DateTime.UtcNow;
        Context.PdfTemplates.Update(existing);
        await Context.SaveChangesAsync();

        return existing;
    }
}
