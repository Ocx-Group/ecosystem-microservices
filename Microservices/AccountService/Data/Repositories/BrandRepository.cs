using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class BrandRepository : BaseRepository, IBrandRepository
{
    public BrandRepository(AccountServiceDbContext context) : base(context) { }

    public Task<Brand?> GetBrandBySecretKeyAsync(string secretKey)
        => Context.Brands.FirstOrDefaultAsync(x => x.SecretKey == secretKey);
}
