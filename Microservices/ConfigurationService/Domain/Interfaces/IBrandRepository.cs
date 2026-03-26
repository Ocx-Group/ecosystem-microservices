using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Domain.Interfaces;

public interface IBrandRepository
{
    Task<Brand?> GetBrandByIdAsync(string secretKey);
}
