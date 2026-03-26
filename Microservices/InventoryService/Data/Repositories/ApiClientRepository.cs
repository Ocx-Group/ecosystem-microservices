using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.InventoryService.Data.Repositories;

public class ApiClientRepository : BaseRepository, IApiClientRepository
{
    public ApiClientRepository(InventoryServiceDbContext context) : base(context) { }

    public Task<bool> ValidateApiClient(string token)
        => Context.ApiClients.AnyAsync(x => x.Token == token);
}
