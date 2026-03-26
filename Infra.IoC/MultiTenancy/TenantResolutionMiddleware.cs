using System.Net;
using System.Text.Json;
using Ecosystem.Domain.Core.MultiTenancy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace Ecosystem.Infra.IoC.MultiTenancy;

/// <summary>
/// Shared middleware that resolves the tenant from X-Client-ID + validates Authorization token.
/// Replaces the per-service TokenValidationMiddleware pattern.
/// </summary>
public class TenantResolutionMiddleware(RequestDelegate next)
{
    private static readonly HashSet<string> SkipPaths = ["/health"];
    private static readonly HashSet<string> SkipPrefixes = ["/hubs/"];

    /// <summary>
    /// Configures additional path prefixes to skip tenant resolution (e.g., SignalR hubs).
    /// Call before using the middleware.
    /// </summary>
    public static void AddSkipPrefix(string prefix) => SkipPrefixes.Add(prefix);

    public async Task InvokeAsync(
        HttpContext context,
        ITenantStore tenantStore,
        IApiTokenValidator tokenValidator,
        ILogger<TenantResolutionMiddleware> logger)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (ShouldSkip(path))
        {
            await next(context);
            return;
        }

        var token = context.Request.Headers.Authorization.ToString();
        var secretKey = context.Request.Headers["X-Client-ID"].ToString();

        if (string.IsNullOrEmpty(token) && context.Request.HasFormContentType)
        {
            var form = await context.Request.ReadFormAsync();
            token = form["Authorization"]!;
        }

        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(secretKey))
        {
            await RespondUnauthorized(context, logger, "Token o ClientID no encontrados");
            return;
        }

        var isValidToken = await tokenValidator.ValidateAsync(token);
        var tenant = await tenantStore.ResolveTenantAsync(secretKey);

        if (!isValidToken || tenant is null)
        {
            await RespondUnauthorized(context, logger, "Token o ClientID inválidos");
            return;
        }

        context.Items["tenantId"] = tenant.Id;
        // Keep backward compatibility with existing code that reads "brandId"
        context.Items["brandId"] = tenant.Id;

        logger.LogInformation("Solicitud aceptada. URL: {Url}, Tenant: {Tenant}",
            context.Request.GetDisplayUrl(), tenant.Name);

        await next(context);
    }

    private static bool ShouldSkip(string path)
    {
        if (SkipPaths.Contains(path))
            return true;

        foreach (var prefix in SkipPrefixes)
        {
            if (path.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private static async Task RespondUnauthorized(HttpContext context, ILogger logger, string reason)
    {
        logger.LogInformation("No autorizado, {Reason}. URL: {Url}",
            reason, context.Request.GetDisplayUrl());

        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        context.Response.ContentType = "application/json";

        var response = new { Success = false, Code = 401, Message = "No autorizado" };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}
