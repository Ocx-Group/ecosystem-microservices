using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class MenuConfigurationRepository : BaseRepository, IMenuConfigurationRepository
{
    public MenuConfigurationRepository(AccountServiceDbContext context) : base(context) { }

    public Task<bool> IsExistMenuConfigurationAsync(int id)
        => Context.MenuConfigurations.AnyAsync(x => x.Id == id);

    public Task<List<MenuConfiguration>> GetAllMenuConfigurationsAsync()
        => Context.MenuConfigurations.AsNoTracking().ToListAsync();
}
