using System.Text.Json;
using Ecosystem.Domain.Core.Caching;
using StackExchange.Redis;

namespace Ecosystem.Infra.Cache;

/// <summary>
/// Redis-backed implementation of ICacheService.
/// Shared across all microservices via DI registration.
/// </summary>
public class RedisCacheService : ICacheService, IDisposable
{
    private readonly IDatabase _db;
    private readonly IConnectionMultiplexer _connectionMultiplexer;
    private readonly JsonSerializerOptions _jsonOptions;
    private bool _disposed;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _connectionMultiplexer = connectionMultiplexer;
        _db = connectionMultiplexer.GetDatabase();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        };
    }

    public Task<bool> KeyExists(string key)
        => _db.KeyExistsAsync(new RedisKey(key));

    public async Task<T?> Get<T>(string key)
    {
        var value = await _db.StringGetAsync(key);

        if (!value.HasValue)
            return default;

        return JsonSerializer.Deserialize<T>(value.ToString(), _jsonOptions);
    }

    public async Task Set<T>(string key, T value, TimeSpan expiration)
    {
        var json = JsonSerializer.Serialize(value, _jsonOptions);
        await _db.StringSetAsync(key, json, expiration);
    }

    public async Task Delete(string key)
        => await _db.KeyDeleteAsync(key);

    public async Task<List<T?>> GetMultiple<T>(List<string> keys)
    {
        var redisKeys = keys.Select(k => new RedisKey(k)).ToArray();
        var values = await _db.StringGetAsync(redisKeys);

        return values
            .Where(v => v.HasValue)
            .Select(v => JsonSerializer.Deserialize<T>(v.ToString(), _jsonOptions))
            .ToList();
    }

    public async Task<T> GetOrSet<T>(string key, TimeSpan expiration, Func<Task<T>> factory)
    {
        var exists = await KeyExists(key);

        if (exists)
        {
            var cached = await Get<T>(key);
            if (cached is not null)
                return cached;
        }

        var value = await factory();
        await Set(key, value, expiration);
        return value;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
            _connectionMultiplexer.Dispose();
        _disposed = true;
    }
}
