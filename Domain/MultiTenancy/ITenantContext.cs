namespace Ecosystem.Domain.Core.MultiTenancy;

/// <summary>
/// Provides the current tenant (brand) context for the request.
/// Injected as Scoped — each HTTP request gets its own tenant resolution.
/// </summary>
public interface ITenantContext
{
    /// <summary>Current brand/tenant ID resolved from the request.</summary>
    long TenantId { get; }
}
