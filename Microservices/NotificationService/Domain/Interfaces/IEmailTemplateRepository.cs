using Ecosystem.NotificationService.Domain.Models;

namespace Ecosystem.NotificationService.Domain.Interfaces;

public interface IEmailTemplateRepository
{
    Task<EmailTemplate?> GetByKeyAndBrandAsync(string templateKey, long brandId);
    Task<ICollection<EmailTemplate>> GetAllAsync();
    Task<ICollection<EmailTemplate>> GetByBrandAsync(long brandId);
    Task<EmailTemplate?> GetByIdAsync(string id);
    Task<EmailTemplate> CreateAsync(EmailTemplate template);
    Task<EmailTemplate> UpdateAsync(EmailTemplate template);
    Task<bool> DeleteAsync(string id);
}
