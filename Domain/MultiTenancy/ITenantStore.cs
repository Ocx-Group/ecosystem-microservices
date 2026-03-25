namespace Ecosystem.Domain.Core.MultiTenancy;

/// <summary>
/// Resolves a tenant by its secret key.
/// Each microservice implements this against its own database.
/// </summary>
public interface ITenantStore
{
    /// <summary>
    /// Resolves a tenant from the provided secret key (X-Client-ID header).
    /// Returns the tenant ID if valid, null if not found.
    /// </summary>
    Task<TenantInfo?> ResolveTenantAsync(string secretKey);
}

/// <summary>Lightweight tenant information resolved from the store.</summary>
public record TenantInfo(long Id, string Name);
