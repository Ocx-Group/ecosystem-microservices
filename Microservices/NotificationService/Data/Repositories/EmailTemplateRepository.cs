using Ecosystem.NotificationService.Data.Context;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using MongoDB.Driver;

namespace Ecosystem.NotificationService.Data.Repositories;

public class EmailTemplateRepository : IEmailTemplateRepository
{
    private readonly MongoDbContext _context;

    public EmailTemplateRepository(MongoDbContext context)
        => _context = context;

    public async Task<EmailTemplate?> GetByKeyAndBrandAsync(string templateKey, long brandId)
        => await _context.EmailTemplates
            .Find(t => t.TemplateKey == templateKey && t.BrandId == brandId && t.IsActive)
            .FirstOrDefaultAsync();

    public async Task<ICollection<EmailTemplate>> GetAllAsync()
        => await _context.EmailTemplates
            .Find(_ => true)
            .SortByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<ICollection<EmailTemplate>> GetByBrandAsync(long brandId)
        => await _context.EmailTemplates
            .Find(t => t.BrandId == brandId)
            .SortByDescending(t => t.CreatedAt)
            .ToListAsync();

    public async Task<EmailTemplate?> GetByIdAsync(string id)
        => await _context.EmailTemplates
            .Find(t => t.Id == id)
            .FirstOrDefaultAsync();

    public async Task<EmailTemplate> CreateAsync(EmailTemplate template)
    {
        template.CreatedAt = DateTime.UtcNow;
        await _context.EmailTemplates.InsertOneAsync(template);
        return template;
    }

    public async Task<EmailTemplate> UpdateAsync(EmailTemplate template)
    {
        template.UpdatedAt = DateTime.UtcNow;
        await _context.EmailTemplates.ReplaceOneAsync(t => t.Id == template.Id, template);
        return template;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _context.EmailTemplates.DeleteOneAsync(t => t.Id == id);
        return result.DeletedCount > 0;
    }
}
