using Ecosystem.NotificationService.Data.Context;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.NotificationService.Data.Repositories;

public class EmailTemplateRepository : IEmailTemplateRepository
{
    private readonly NotificationServiceDbContext _context;

    public EmailTemplateRepository(NotificationServiceDbContext context)
        => _context = context;

    public async Task<EmailTemplate?> GetByKeyAndBrandAsync(string templateKey, long brandId)
        => await _context.EmailTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TemplateKey == templateKey && t.BrandId == brandId && t.IsActive);

    public async Task<ICollection<EmailTemplate>> GetAllAsync()
        => await _context.EmailTemplates
            .AsNoTracking()
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<ICollection<EmailTemplate>> GetByBrandAsync(long brandId)
        => await _context.EmailTemplates
            .AsNoTracking()
            .Where(t => t.BrandId == brandId)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<EmailTemplate?> GetByIdAsync(long id)
        => await _context.EmailTemplates.FindAsync(id);

    public async Task<EmailTemplate> CreateAsync(EmailTemplate template)
    {
        template.CreatedAt = DateTime.UtcNow;
        _context.EmailTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task<EmailTemplate> UpdateAsync(EmailTemplate template)
    {
        template.UpdatedAt = DateTime.UtcNow;
        _context.EmailTemplates.Update(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var template = await _context.EmailTemplates.FindAsync(id);
        if (template is null) return false;
        _context.EmailTemplates.Remove(template);
        await _context.SaveChangesAsync();
        return true;
    }
}
