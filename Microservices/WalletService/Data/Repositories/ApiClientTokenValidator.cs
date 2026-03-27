using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.WalletService.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.WalletService.Data.Repositories;

public class ApiClientTokenValidator : IApiTokenValidator
{
    private readonly WalletServiceDbContext _context;

    public ApiClientTokenValidator(WalletServiceDbContext context)
        => _context = context;

    public Task<bool> ValidateAsync(string token)
        => _context.ApiClient.AnyAsync(x => x.Token == token);
}
