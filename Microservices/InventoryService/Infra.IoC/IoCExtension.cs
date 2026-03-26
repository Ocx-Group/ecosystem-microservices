using Ecosystem.InventoryService.Application.Mappings;
using Ecosystem.InventoryService.Data.Context;
using Ecosystem.InventoryService.Data.Repositories;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.Infra.IoC.MultiTenancy;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BrandTenantStore = Ecosystem.InventoryService.Data.Repositories.BrandTenantStore;
using ApiClientTokenValidator = Ecosystem.InventoryService.Data.Repositories.ApiClientTokenValidator;

namespace Ecosystem.InventoryService.Infra.IoC;

public static class IoCExtension
{
    public static void AddInventoryServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInventoryServiceDbContext(configuration);
        services.AddMultiTenancy<BrandTenantStore, ApiClientTokenValidator>();
        services.InjectAutoMapper();
        services.InjectMediatR();
        services.InjectValidators();
        services.InjectRepositories();
    }

    private static void InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductInventoryRepository, ProductInventoryRepository>();
        services.AddScoped<IProductCategoryRepository, ProductCategoryRepository>();
        services.AddScoped<IProductAttributeRepository, ProductAttributeRepository>();
        services.AddScoped<IProductAttributeValueRepository, ProductAttributeValueRepository>();
        services.AddScoped<IProductDiscountRepository, ProductDiscountRepository>();
        services.AddScoped<IProductBannerRepository, ProductBannerRepository>();
        services.AddScoped<IProductCombinationRepository, ProductCombinationRepository>();
        services.AddScoped<IApiClientRepository, ApiClientRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
    }

    private static void InjectMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(InventoryMappingProfile).Assembly));
    }

    private static void InjectValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(InventoryMappingProfile).Assembly);
    }

    private static void InjectAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(InventoryMappingProfile).Assembly);
    }

    private static void AddInventoryServiceDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        services.AddDbContext<InventoryServiceDbContext>(options =>
            options.UseNpgsql(connectionString));
    }
}
