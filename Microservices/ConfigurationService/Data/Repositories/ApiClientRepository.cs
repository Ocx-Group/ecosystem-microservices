using Microsoft.EntityFrameworkCore;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Domain.Interfaces;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class ApiClientRepository : BaseRepository, IApiClientRepository
{
    public ApiClientRepository(ConfigurationServiceDbContext context) : base(context) { }

    public Task<bool> ValidateApiClient(string token)
        => Context.ApiClients.AnyAsync(x => x.Token == token);
}
