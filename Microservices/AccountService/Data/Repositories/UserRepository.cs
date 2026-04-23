using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(AccountServiceDbContext context) : base(context) { }

    public async Task<bool> DeleteUserAsync(User user)
    {
        user.DeletedAt = DateTime.Now;
        Context.Users.Update(user);
        await Context.SaveChangesAsync();
        return true;
    }

    public Task<List<User>> GetUsersAsync(long brandId)
        => Context.Users.Where(x => x.BrandId == brandId).Include(x => x.Rol).ToListAsync();

    public Task<User?> GetUserByEmailAsync(string email, long brandId)
        => Context.Users.FirstOrDefaultAsync(x => x.Email == email && x.BrandId == brandId);

    public Task<User?> GetUserByUserNameAsync(string userName, long brandId)
        => Context.Users.Include(x => x.Rol).FirstOrDefaultAsync(x => x.Username == userName && x.BrandId == brandId);

    public Task<User?> GetUserByIdAsync(int id, long brandId)
        => Context.Users
            .Include(x => x.Rol)
            .FirstOrDefaultAsync(x => x.Id == id && x.BrandId == brandId);

    public Task<List<User>> GetUserByRolIdAsync(int id, long brandId)
        => Context.Users.Where(x => x.RolId == id && x.BrandId == brandId).ToListAsync();

    public async Task<User> UpdateUserAsync(User user)
    {
        user.UpdatedAt = DateTime.Now;
        Context.Users.Update(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<User> CreateUserAsync(User user)
    {
        var today = DateTime.Now;
        user.CreatedAt = today;
        user.UpdatedAt = today;
        await Context.Users.AddAsync(user);
        await Context.SaveChangesAsync();
        return user;
    }

    public async Task<User> UpdateUserImage(User user)
    {
        user.UpdatedAt = DateTime.Now;
        Context.Users.Update(user);
        await Context.SaveChangesAsync();
        return user;
    }
}
