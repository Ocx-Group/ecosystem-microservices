using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetUsersAsync(long brandId);
    Task<User?> GetUserByIdAsync(int id, long brandId);
    Task<List<User>> GetUserByRolIdAsync(int id, long brandId);
    Task<User?> GetUserByEmailAsync(string email, long brandId);
    Task<User> UpdateUserAsync(User user);
    Task<User> CreateUserAsync(User user);
    Task<bool> DeleteUserAsync(User user);
    Task<User?> GetUserByUserNameAsync(string userName, long brandId);
    Task<User> UpdateUserImage(User user);
}
