namespace Ecosystem.Domain.Core.Caching;

/// <summary>
/// Shared cache abstraction used by all microservices.
/// Implementation provided by Ecosystem.Infra.Cache (Redis-backed).
/// </summary>
public interface ICacheService
{
    Task<bool> KeyExists(string key);
    Task<T?> Get<T>(string key);
    Task Set<T>(string key, T value, TimeSpan expiration);
    Task Delete(string key);
    Task<List<T?>> GetMultiple<T>(List<string> keys);

    /// <summary>
    /// Gets a value from cache, or computes and stores it if not present.
    /// </summary>
    Task<T> GetOrSet<T>(string key, TimeSpan expiration, Func<Task<T>> factory);
}
