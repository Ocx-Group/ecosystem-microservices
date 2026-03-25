using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IRoleRepository
{
    Task<List<Role>> GetRolesAsync();
    Task<Role?> GetRoleByIdAsync(int id);
    Task<bool> IsExistRoleAsync(int id);
    Task<Role> UpdateRoleAsync(Role role);
    Task<Role> CreateRoleAsync(Role role);
    Task<bool> DeleteRoleAsync(Role role);
}
