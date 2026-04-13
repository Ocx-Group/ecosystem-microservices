using AutoMapper;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.Caching;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Extensions;

/// <summary>
/// ConfigurationService implementation of IBrandConfigurationProvider.
/// Reads directly from the database with Redis caching.
/// </summary>
public class BrandConfigurationProvider : IBrandConfigurationProvider
{
    private readonly IBrandConfigurationRepository _repository;
    private readonly ICacheService _cache;
    private readonly ILogger<BrandConfigurationProvider> _logger;
    private readonly IMapper _mapper;

    private const string CachePrefix = "brand_config";
    private const string AllConfigsCacheKey = $"{CachePrefix}:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public BrandConfigurationProvider(
        IBrandConfigurationRepository repository,
        ICacheService cache,
        ILogger<BrandConfigurationProvider> logger,
        IMapper mapper)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<BrandConfigurationDto?> GetByBrandIdAsync(long brandId)
    {
        var cacheKey = $"{CachePrefix}:{brandId}";

        return await _cache.GetOrSet(cacheKey, CacheDuration, async () =>
        {
            var entity = await _repository.GetByBrandIdAsync(brandId);
            if (entity is null) return null!;

            return _mapper.Map<BrandConfigurationDto>(entity);
        });
    }

    public async Task<IReadOnlyList<BrandConfigurationDto>> GetAllAsync()
    {
        return await _cache.GetOrSet(AllConfigsCacheKey, CacheDuration, async () =>
        {
            var entities = await _repository.GetAllAsync();
            return (IReadOnlyList<BrandConfigurationDto>)entities
                .Select(e => _mapper.Map<BrandConfigurationDto>(e))
                .ToList()
                .AsReadOnly();
        });
    }

    public async Task InvalidateCacheAsync(long? brandId = null)
    {
        if (brandId.HasValue)
        {
            await _cache.Delete($"{CachePrefix}:{brandId.Value}");
            _logger.LogInformation("Invalidated brand config cache for BrandId {BrandId}", brandId.Value);
        }

        // Always invalidate the "all" cache
        await _cache.Delete(AllConfigsCacheKey);
    }
}
