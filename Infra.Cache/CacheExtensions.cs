using Ecosystem.Domain.Core.Caching;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Ecosystem.Infra.Cache;

public static class CacheExtensions
{
    /// <summary>
    /// Registers the shared Redis-backed ICacheService.
    /// Call from each microservice's IoC: services.AddSharedCache(connectionString).
    /// </summary>
    public static IServiceCollection AddSharedCache(this IServiceCollection services, string redisConnectionString)
    {
        services.AddSingleton<IConnectionMultiplexer>(_ =>
            ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }
}
