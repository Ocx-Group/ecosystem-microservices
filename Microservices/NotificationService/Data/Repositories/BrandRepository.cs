using Ecosystem.NotificationService.Data.Context;
using Ecosystem.NotificationService.Domain.Interfaces;
using Ecosystem.NotificationService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.NotificationService.Data.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly NotificationServiceDbContext _context;

    public BrandRepository(NotificationServiceDbContext context)
        => _context = context;

    public Task<Brand?> GetBrandBySecretKeyAsync(string secretKey)
        => _context.Brands.AsNoTracking().FirstOrDefaultAsync(b => b.SecretKey == secretKey);
}
