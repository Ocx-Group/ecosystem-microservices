using Ecosystem.ConfigurationService.Application.Extensions;
using Ecosystem.ConfigurationService.Application.Mappings;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.ConfigurationService.Data.Repositories;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.BrandConfiguration;
using Ecosystem.Infra.IoC.MultiTenancy;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BrandTenantStore = Ecosystem.ConfigurationService.Data.Repositories.BrandTenantStore;
using ApiClientTokenValidator = Ecosystem.ConfigurationService.Data.Repositories.ApiClientTokenValidator;

namespace Ecosystem.ConfigurationService.Infra.IoC;

public static class IoCExtension
{
    public static void AddConfigurationServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConfigurationServiceDbContext(configuration);
        services.AddMultiTenancy<BrandTenantStore, ApiClientTokenValidator>();
        services.InjectAutoMapper();
        services.InjectMediatR();
        services.InjectValidators();
        services.InjectRepositories();
    }

    private static void InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped<IConceptConfigurationRepository, ConceptConfigurationRepository>();
        services.AddScoped<IConceptRepository, ConceptRepository>();
        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<IPaymentGroupRepository, PaymentGroupRepository>();
        services.AddScoped<IIncentiveRepository, IncentiveRepository>();
        services.AddScoped<IGradingRepository, GradingRepository>();
        services.AddScoped<IApiClientRepository, ApiClientRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IMatrixConfigurationRepository, MatrixConfigurationRepository>();
        services.AddScoped<IBrandConfigurationRepository, BrandConfigurationRepository>();
        services.AddScoped<IPdfTemplateRepository, PdfTemplateRepository>();
        services.AddScoped<IBrandConfigurationProvider, BrandConfigurationProvider>();
    }

    private static void InjectMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(ConfigurationMappingProfile).Assembly));
    }

    private static void InjectValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(ConfigurationMappingProfile).Assembly);
    }

    private static void InjectAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ConfigurationMappingProfile).Assembly);
    }

    private static void AddConfigurationServiceDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        services.AddDbContext<ConfigurationServiceDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
