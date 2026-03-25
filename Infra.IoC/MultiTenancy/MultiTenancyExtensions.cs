using Ecosystem.Domain.Core.MultiTenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ecosystem.Infra.IoC.MultiTenancy;

public static class MultiTenancyExtensions
{
    /// <summary>
    /// Registers the shared multi-tenancy services.
    /// Each microservice must also register its own ITenantStore and IApiTokenValidator implementations.
    /// </summary>
    /// <typeparam name="TTenantStore">Service-specific ITenantStore implementation.</typeparam>
    /// <typeparam name="TTokenValidator">Service-specific IApiTokenValidator implementation.</typeparam>
    public static IServiceCollection AddMultiTenancy<TTenantStore, TTokenValidator>(this IServiceCollection services)
        where TTenantStore : class, ITenantStore
        where TTokenValidator : class, IApiTokenValidator
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITenantContext, HttpContextTenantContext>();
        services.AddScoped<ITenantStore, TTenantStore>();
        services.AddScoped<IApiTokenValidator, TTokenValidator>();
        return services;
    }

    /// <summary>
    /// Adds the tenant resolution middleware to the pipeline.
    /// Must be called after authentication and before controllers.
    /// </summary>
    public static IApplicationBuilder UseTenantResolution(this IApplicationBuilder app)
    {
        return app.UseMiddleware<TenantResolutionMiddleware>();
    }
}
