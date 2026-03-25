using Ecosystem.Domain.Core.MultiTenancy;
using Microsoft.AspNetCore.Http;

namespace Ecosystem.Infra.IoC.MultiTenancy;

/// <summary>
/// Reads the tenant ID set by TenantResolutionMiddleware from HttpContext.Items.
/// Registered as Scoped — lives for the duration of the HTTP request.
/// </summary>
public class HttpContextTenantContext : ITenantContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpContextTenantContext(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public long TenantId
    {
        get
        {
            var tenantId = _httpContextAccessor.HttpContext?.Items["tenantId"];
            return tenantId is null ? 0 : Convert.ToInt64(tenantId);
        }
    }
}
