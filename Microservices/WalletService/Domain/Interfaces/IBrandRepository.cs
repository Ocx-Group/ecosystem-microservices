using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IBrandRepository
{
    Task<Brand?> GetBrandByIdAsync(string secretKey);
}