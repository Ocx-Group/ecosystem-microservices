using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class BrandRepository : BaseRepository, IBrandRepository
{
    public BrandRepository(ConfigurationServiceDbContext context) : base(context) { }

    public Task<Brand?> GetBrandByIdAsync(string secretKey)
        => Context.Brands.FirstOrDefaultAsync(x => x.SecretKey == secretKey);
}
