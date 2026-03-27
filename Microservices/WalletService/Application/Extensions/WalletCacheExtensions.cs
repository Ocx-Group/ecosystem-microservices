using Ecosystem.Domain.Core.Caching;
using Ecosystem.WalletService.Domain.Constants;

namespace Ecosystem.WalletService.Application.Extensions;

/// <summary>
/// Wallet-specific cache extension methods built on top of the shared ICacheService.
/// </summary>
public static class WalletCacheExtensions
{
    /// <summary>
    /// Invalidates all balance-related cache keys for the given affiliate IDs.
    /// </summary>
    public static async Task InvalidateBalanceAsync(this ICacheService cache, params int[] affiliateIds)
    {
        foreach (var affiliateId in affiliateIds)
        {
            var keys = new[]
            {
                string.Format(CacheKeys.BalanceInformationModel1A, affiliateId),
                string.Format(CacheKeys.BalanceInformationModel1B, affiliateId),
                string.Format(CacheKeys.BalanceInformationModel2, affiliateId)
            };

            foreach (var key in keys)
            {
                if (await cache.KeyExists(key))
                    await cache.Delete(key);
            }
        }
    }
}
