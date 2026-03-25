using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class RoleRepository : BaseRepository, IRoleRepository
{
    public RoleRepository(AccountServiceDbContext context) : base(context) { }

    public Task<bool> IsExistRoleAsync(int id)
        => Context.Roles.AnyAsync(x => x.Id == id);

    public Task<List<Role>> GetRolesAsync()
        => Context.Roles.Include(x => x.Users).Include(x => x.Privileges).ToListAsync();

    public Task<Role?> GetRoleByIdAsync(int id)
        => Context.Roles.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<bool> DeleteRoleAsync(Role role)
    {
        role.DeletedAt = DateTime.Now;
        await Context.SaveChangesAsync();
        return true;
    }

    public async Task<Role> UpdateRoleAsync(Role role)
    {
        Context.Roles.Update(role);
        await Context.SaveChangesAsync();
        return role;
    }

    public async Task<Role> CreateRoleAsync(Role role)
    {
        await Context.AddAsync(role);
        await Context.SaveChangesAsync();
        return role;
    }
}
