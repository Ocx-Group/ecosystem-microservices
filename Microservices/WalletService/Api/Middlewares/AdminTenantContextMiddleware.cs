using System.Security.Claims;

namespace Ecosystem.WalletService.Api.Middlewares;

/// <summary>
/// Publishes the brand of an authenticated admin JWT as the request tenant, which is
/// where <c>HttpContextTenantContext</c> reads it from.
///
/// Requests without an admin token are left untouched, so the legacy X-Client-ID
/// resolution done by <c>TenantResolutionMiddleware</c> keeps working for every other
/// wallet endpoint.
/// </summary>
public sealed class AdminTenantContextMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var rawBrandId = context.User.FindFirstValue("brand_id");
            if (long.TryParse(rawBrandId, out var brandId) && brandId > 0)
            {
                context.Items["tenantId"] = brandId;
                context.Items["brandId"] = brandId;
            }
        }

        await next(context);
    }
}
