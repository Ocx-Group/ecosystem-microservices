using AutoMapper;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Domain.Core.Caching;
using Ecosystem.Grpc.Configuration;
using Ecosystem.WalletService.Domain.Responses;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Adapters;

public class GrpcConfigurationAdapter : IConfigurationAdapter
{
    private readonly ConfigurationGrpc.ConfigurationGrpcClient _client;
    private readonly IMapper _mapper;
    private readonly ICacheService _cache;
    private readonly ILogger<GrpcConfigurationAdapter> _logger;

    /// <summary>
    /// How long a brand configuration survives here without being re-read. Short compared
    /// with ConfigurationService's own 24 hours, because this copy is only invalidated by
    /// a message from the other service: the TTL is what limits the damage if one is ever
    /// missed, and re-reading a handful of times an hour costs nothing.
    /// </summary>
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public GrpcConfigurationAdapter(
        ConfigurationGrpc.ConfigurationGrpcClient client,
        IMapper mapper,
        ICacheService cache,
        ILogger<GrpcConfigurationAdapter> logger)
    {
        _client = client;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
    }

    public async Task<MatrixConfiguration?> GetMatrixConfiguration(long brandId, int matrixType)
    {
        try
        {
            var response = await _client.GetMatrixConfigurationAsync(new GetMatrixConfigurationRequest
            {
                BrandId = brandId,
                MatrixType = matrixType
            });
            if (!response.Success || response.Configuration is null) return null;
            return _mapper.Map<MatrixConfiguration>(response.Configuration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetMatrixConfiguration for type {MatrixType}", matrixType);
            return null;
        }
    }

    public async Task<List<MatrixConfiguration>?> GetAllMatrixConfigurations(long brandId)
    {
        try
        {
            var response = await _client.GetAllMatrixConfigurationsAsync(new GetAllMatrixConfigurationsRequest
            {
                BrandId = brandId
            });
            if (!response.Success) return null;
            return response.Configurations.Select(_mapper.Map<MatrixConfiguration>).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "gRPC error in GetAllMatrixConfigurations");
            return null;
        }
    }

    /// <summary>
    /// Reads the brand configuration, preferring the shared Redis copy over a gRPC round
    /// trip. Every wallet operation needs this, so without the cache each one opened a
    /// call to ConfigurationService — more latency, and one more chance to land on a
    /// dropped HTTP/2 connection.
    ///
    /// The entry is dropped by ConfigurationService itself: every handler that changes a
    /// brand calls <c>InvalidateCacheAsync</c>, which deletes this key alongside its own.
    /// </summary>
    public async Task<BrandConfigurationDto?> GetBrandConfiguration(
        long brandId,
        CancellationToken cancellationToken = default)
    {
        var cacheKey = BrandConfigurationCacheKeys.Downstream(brandId);
        var cached = await _cache.Get<BrandConfigurationDto>(cacheKey);

        if (cached is not null)
            return cached;

        try
        {
            var response = await _client.GetBrandConfigurationAsync(
                new GetBrandConfigurationRequest { BrandId = brandId },
                deadline: DateTime.UtcNow.AddSeconds(10),
                cancellationToken: cancellationToken);

            if (!response.Success || response.Configuration is null)
            {
                _logger.LogWarning(
                    "Brand configuration not found for brand {BrandId}: {Message}",
                    brandId,
                    response.Message);
                return null;
            }

            var configuration = MapBrandConfiguration(response.Configuration);

            // A miss is never cached: an unknown brand is usually a misrouted request, and
            // remembering it would keep the brand broken after it is created.
            await _cache.Set(cacheKey, configuration, CacheDuration);

            return configuration;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "gRPC error retrieving brand configuration for brand {BrandId}",
                brandId);
            throw;
        }
    }

    private static BrandConfigurationDto MapBrandConfiguration(
        BrandConfigurationMessage source) =>
        new()
        {
            BrandId = source.BrandId,
            Name = source.Name,
            AdminUserName = source.AdminUserName,
            SenderName = source.SenderName,
            SenderEmail = source.SenderEmail,
            EmailTemplateFolder = source.EmailTemplateFolder,
            ClientUrl = source.ClientUrl,
            CommissionEnabled = source.CommissionEnabled,
            CommissionLevels = source.CommissionLevels
                .Select(Convert.ToDecimal)
                .ToArray(),
            BonusPercentage = Convert.ToDecimal(source.BonusPercentage),
            DailyBonusAlwaysDistribute = source.DailyBonusAlwaysDistribute,
            MonthlyCommissionEnabled = source.MonthlyCommissionEnabled,
            MonthlyCommissionInterestRate =
                Convert.ToDecimal(source.MonthlyCommissionInterestRate),
            MonthlyCommissionWaitingDays = source.MonthlyCommissionWaitingDays,
            MonthlyCommissionPaymentGroupId = source.HasMonthlyCommissionPaymentGroupId
                ? source.MonthlyCommissionPaymentGroupId
                : null,
            // A proto3 string defaults to "" rather than null, and an unrecognised value
            // means the same thing here: fall back to the long-standing procedure.
            MonthlyCommissionSource = MonthlyCommissionSources.IsInvoiceTotal(source.MonthlyCommissionSource)
                ? MonthlyCommissionSources.InvoiceTotal
                : MonthlyCommissionSources.PaymentGroup,
            PdfTemplateName = source.PdfTemplateName,
            CompanyName = source.CompanyName,
            CompanyIdentifier = source.HasCompanyIdentifier ? source.CompanyIdentifier : null,
            SupportEmail = source.SupportEmail,
            SupportPhone = source.HasSupportPhone ? source.SupportPhone : null,
            DocumentType = source.HasDocumentType ? source.DocumentType : null,
            LogoUrl = source.HasLogoUrl ? source.LogoUrl : null,
            PrimaryColor = source.PrimaryColor,
            SecondaryColor = source.SecondaryColor,
            BackgroundColor = source.BackgroundColor,
            DefaultFatherAffiliateId = source.HasDefaultFatherAffiliateId
                ? source.DefaultFatherAffiliateId
                : null,
            ActivateOnRegistration = source.ActivateOnRegistration,
            DefaultPaymentGroupId = source.HasDefaultPaymentGroupId
                ? source.DefaultPaymentGroupId
                : null,
            TradingAcademyPaymentGroupId = source.HasTradingAcademyPaymentGroupId
                ? source.TradingAcademyPaymentGroupId
                : null,
            WithdrawalValidationType = source.WithdrawalValidationType,
            WithdrawalTimeZone = source.HasWithdrawalTimeZone
                ? source.WithdrawalTimeZone
                : null,
            WithdrawalStartHour = source.HasWithdrawalStartHour
                ? source.WithdrawalStartHour
                : null,
            WithdrawalEndHour = source.HasWithdrawalEndHour
                ? source.WithdrawalEndHour
                : null,
            WithdrawalCapNoDirects = source.HasWithdrawalCapNoDirects
                ? Convert.ToDecimal(source.WithdrawalCapNoDirects)
                : null,
            Requires10PercentPurchaseRule = source.Requires10PercentPurchaseRule,
            PoolValidationRequired = source.PoolValidationRequired,
            ConPaymentEnabled = source.ConPaymentEnabled,
            ConPaymentAddress = source.HasConPaymentAddress ? source.ConPaymentAddress : null,
            BlockchainNetworkId = source.HasBlockchainNetworkId
                ? source.BlockchainNetworkId
                : null,
            IsActive = source.IsActive
        };
}
