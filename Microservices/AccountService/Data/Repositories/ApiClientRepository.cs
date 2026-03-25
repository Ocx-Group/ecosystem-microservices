using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class ApiClientRepository : BaseRepository, IApiClientRepository
{
    public ApiClientRepository(AccountServiceDbContext context) : base(context) { }

    public Task<bool> ValidateApiClient(string token)
        => Context.ApiClients.AnyAsync(x => x.Token == token);
}
