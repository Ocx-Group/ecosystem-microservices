namespace Ecosystem.Domain.Core.BrandConfiguration;

/// <summary>
/// Redis keys holding brand configuration. They live here, in the shared kernel, because
/// two sides of the system touch them: ConfigurationService writes and invalidates, and
/// every downstream service reads the copy it fetched over gRPC. All services share one
/// Redis instance (the same <c>redis-credentials</c> secret) with no per-service prefix,
/// so the names have to agree.
/// </summary>
public static class BrandConfigurationCacheKeys
{
    private const string Prefix = "brand_config";

    /// <summary>Every brand configuration, as ConfigurationService reads them from its own database.</summary>
    public const string All = $"{Prefix}:all";

    /// <summary>
    /// ConfigurationService's own copy, mapped from the entity.
    /// </summary>
    public static string Own(long brandId) => $"{Prefix}:{brandId}";

    /// <summary>
    /// The copy a downstream service caches after a gRPC call. Deliberately a different
    /// key from <see cref="Own"/>: the two are built by different mappings, and letting a
    /// downstream service overwrite ConfigurationService's own entry would let any field
    /// the proto happens to drop propagate back as authoritative.
    ///
    /// One key for all downstream services, since they all build the DTO from the same
    /// <c>BrandConfigurationMessage</c>.
    /// </summary>
    public static string Downstream(long brandId) => $"{Prefix}:downstream:{brandId}";
}
