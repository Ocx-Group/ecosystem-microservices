namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Abstraction over Redis cache operations
public interface ICacheService
{
    Task<bool> KeyExists(string key);
    Task<T?> Get<T>(string key);
    Task Set<T>(string key, T value, TimeSpan expiration);
    Task Delete(string key);
    Task InvalidateBalanceAsync(params int[] affiliateIds);
}
