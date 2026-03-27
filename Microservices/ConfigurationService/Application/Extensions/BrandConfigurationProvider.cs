using System.Text.Json;
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

    private const string CachePrefix = "brand_config";
    private const string AllConfigsCacheKey = $"{CachePrefix}:all";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public BrandConfigurationProvider(
        IBrandConfigurationRepository repository,
        ICacheService cache,
        ILogger<BrandConfigurationProvider> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<BrandConfigurationDto?> GetByBrandIdAsync(long brandId)
    {
        var cacheKey = $"{CachePrefix}:{brandId}";

        return await _cache.GetOrSet(cacheKey, CacheDuration, async () =>
        {
            var entity = await _repository.GetByBrandIdAsync(brandId);
            if (entity is null) return null!;

            return MapToDto(entity);
        });
    }

    public async Task<IReadOnlyList<BrandConfigurationDto>> GetAllAsync()
    {
        return await _cache.GetOrSet(AllConfigsCacheKey, CacheDuration, async () =>
        {
            var entities = await _repository.GetAllAsync();
            return (IReadOnlyList<BrandConfigurationDto>)entities
                .Select(MapToDto)
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

    private static BrandConfigurationDto MapToDto(Domain.Models.BrandConfiguration entity)
    {
        var commissionLevels = string.IsNullOrEmpty(entity.CommissionLevelsJson)
            ? []
            : JsonSerializer.Deserialize<decimal[]>(entity.CommissionLevelsJson) ?? [];

        return new BrandConfigurationDto
        {
            BrandId = entity.BrandId,
            Name = entity.Brand?.Name ?? string.Empty,
            AdminUserName = entity.AdminUserName,
            SenderName = entity.SenderName,
            SenderEmail = entity.SenderEmail,
            EmailTemplateFolder = entity.EmailTemplateFolder,
            ClientUrl = entity.ClientUrl,
            CommissionEnabled = entity.CommissionEnabled,
            CommissionLevels = commissionLevels,
            BonusPercentage = entity.BonusPercentage,
            PdfTemplateName = entity.PdfTemplateName,
            CompanyName = entity.CompanyName,
            CompanyIdentifier = entity.CompanyIdentifier,
            SupportEmail = entity.SupportEmail,
            SupportPhone = entity.SupportPhone,
            DocumentType = entity.DocumentType,
            LogoUrl = entity.LogoUrl,
            PrimaryColor = entity.PrimaryColor,
            SecondaryColor = entity.SecondaryColor,
            BackgroundColor = entity.BackgroundColor,
            DefaultFatherAffiliateId = entity.DefaultFatherAffiliateId,
            ActivateOnRegistration = entity.ActivateOnRegistration,
            DefaultPaymentGroupId = entity.DefaultPaymentGroupId,
            TradingAcademyPaymentGroupId = entity.TradingAcademyPaymentGroupId,
            WithdrawalValidationType = entity.WithdrawalValidationType,
            WithdrawalTimeZone = entity.WithdrawalTimeZone,
            WithdrawalStartHour = entity.WithdrawalStartHour,
            WithdrawalEndHour = entity.WithdrawalEndHour,
            WithdrawalCapNoDirects = entity.WithdrawalCapNoDirects,
            Requires10PercentPurchaseRule = entity.Requires10PercentPurchaseRule,
            PoolValidationRequired = entity.PoolValidationRequired,
            ConPaymentEnabled = entity.ConPaymentEnabled,
            ConPaymentAddress = entity.ConPaymentAddress,
            BlockchainNetworkId = entity.BlockchainNetworkId,
            IsActive = entity.IsActive
        };
    }
}
