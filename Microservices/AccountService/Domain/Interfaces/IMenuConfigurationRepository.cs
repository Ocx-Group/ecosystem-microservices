using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IMenuConfigurationRepository
{
    Task<List<MenuConfiguration>> GetAllMenuConfigurationsAsync();
    Task<bool> IsExistMenuConfigurationAsync(int id);
}
