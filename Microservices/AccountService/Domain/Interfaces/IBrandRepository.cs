using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IBrandRepository
{
    Task<Brand?> GetBrandBySecretKeyAsync(string secretKey);
}
