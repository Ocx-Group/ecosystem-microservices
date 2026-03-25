using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IPrivilegeRepository
{
    Task<List<Privilege>> GetAllPrivileges(int rolId);
    Task<Privilege> UpdatePrivilegeAsync(Privilege privilege);
    Task<Privilege> CreatePrivilegeAsync(Privilege privilege);
    Task<Privilege?> GetPrivilegeByIdAsync(int id);
    Task<List<Privilege>> GetPrivilegesAsync();
}
