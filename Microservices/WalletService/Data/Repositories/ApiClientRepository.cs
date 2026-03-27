using Microsoft.EntityFrameworkCore;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Domain.Interfaces;

namespace Ecosystem.WalletService.Data.Repositories;

public class ApiClientRepository : BaseRepository, IApiClientRepository
{
    public ApiClientRepository(WalletServiceDbContext context) : base(context) { }


    public Task<bool> ValidateApiClient(string token)
        => Context.ApiClient.AnyAsync(x => x.Token == token);

}