using System.Security.Claims;

namespace Ecosystem.ConfigurationService.Api.Middlewares;

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
