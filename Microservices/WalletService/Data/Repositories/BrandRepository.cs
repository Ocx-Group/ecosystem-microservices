using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class BrandRepository : BaseRepository, IBrandRepository
{
    public BrandRepository(WalletServiceDbContext context) : base(context)
    {
    }

    public Task<Brand?> GetBrandByIdAsync(string secretKey)
        => Context.Brand.FirstOrDefaultAsync(x => x.SecretKey == secretKey);
}