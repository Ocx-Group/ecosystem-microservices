using Ecosystem.NotificationService.Data.Context;
using Ecosystem.NotificationService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.NotificationService.Data.Repositories;

public class ApiClientRepository : IApiClientRepository
{
    private readonly NotificationServiceDbContext _context;

    public ApiClientRepository(NotificationServiceDbContext context)
        => _context = context;

    public Task<bool> ValidateApiClientAsync(string token)
        => _context.ApiClients.AsNoTracking().AnyAsync(c => c.Token == token);
}
