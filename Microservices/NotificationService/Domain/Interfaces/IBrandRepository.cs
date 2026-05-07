using Ecosystem.NotificationService.Domain.Models;

namespace Ecosystem.NotificationService.Domain.Interfaces;

public interface IBrandRepository
{
    Task<Brand?> GetBrandBySecretKeyAsync(string secretKey);
}
