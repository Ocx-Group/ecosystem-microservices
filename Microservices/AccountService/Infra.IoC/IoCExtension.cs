using Ecosystem.AccountService.Application.Consumers;
using Ecosystem.AccountService.Application.Mappings;
using Ecosystem.AccountService.Application.Validators.Auth;
using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Data.Repositories;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.Domain.Core.Bus;
using Ecosystem.Infra.IoC.MultiTenancy;
using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecosystem.AccountService.Infra.IoC;

public static class IoCExtension
{
    public static void AddAccountServiceDependencies(
        this IServiceCollection services,
        IConfiguration configuration,
        string rabbitMqHost,
        string rabbitMqUsername,
        string rabbitMqPassword)
    {
        services.AddAccountServiceDbContext(configuration);
        services.AddMultiTenancy<BrandTenantStore, ApiClientTokenValidator>();
        services.AddMassTransitWithConsumers(rabbitMqHost, rabbitMqUsername, rabbitMqPassword);
        services.InjectAutoMapper();
        services.InjectMediatR();
        services.InjectValidators();
        services.InjectRepositories();
        services.InjectServices(configuration);
    }

    private static void AddMassTransitWithConsumers(
        this IServiceCollection services,
        string rabbitMqHost,
        string username,
        string password)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<UpdateActivationDateConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitMqHost, h =>
                {
                    h.Username(username);
                    h.Password(password);
                });
                cfg.ConfigureEndpoints(context);
            });
        });

        services.AddScoped<IEventBus, Ecosystem.Infra.Bus.MassTransitBus>();
    }

    private static void InjectRepositories(this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserAffiliateInfoRepository, UserAffiliateInfoRepository>();
        services.AddScoped<ILoginMovementsRepository, LoginMovementsRepository>();
        services.AddScoped<IMasterPasswordRepository, MasterPasswordRepository>();
        services.AddScoped<IApiClientRepository, ApiClientRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IPrivilegeRepository, PrivilegeRepository>();
        services.AddScoped<IMenuConfigurationRepository, MenuConfigurationRepository>();
        services.AddScoped<IMatrixPositionsRepository, MatrixPositionsRepository>();
        services.AddScoped<IMatrixCyclesRepository, MatrixCyclesRepository>();
        services.AddScoped<ITicketRepository, TicketRepository>();
        services.AddScoped<ITicketMessageRepository, TicketMessageRepository>();
        services.AddScoped<ITicketCategoriesRepository, TicketCategoriesRepository>();
        services.AddScoped<IAffiliateBtcRepository, AffiliateBtcRepository>();
        services.AddScoped<IAffiliateAddressRepository, AffiliateAddressRepository>();
        services.AddScoped<ILeaderBoardModel4Repository, LeaderBoardModel4Repository>();
        services.AddScoped<ILeaderBoardModel5Repository, LeaderBoardModel5Repository>();
        services.AddScoped<ILeaderBoardModel6Repository, LeaderBoardModel6Repository>();
    }

    private static void InjectMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Auth.UserAuthenticationCommand).Assembly));
    }

    private static void InjectValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(UserAuthenticationValidator).Assembly);
    }

    private static void InjectAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);
    }

    private static void AddAccountServiceDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        services.AddDbContext<AccountServiceDbContext>(options =>
            options.UseNpgsql(connectionString));
    }

    private static void InjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Application.Services.BlockchainSettings>(
            configuration.GetSection(Application.Services.BlockchainSettings.SectionName));
        services.AddScoped<IBlockchainService, Application.Services.BlockchainService>();

        services.Configure<Application.Settings.AccountServiceSettings>(
            configuration.GetSection(Application.Settings.AccountServiceSettings.SectionName));
        services.AddScoped<Application.Adapters.IWalletServiceAdapter, Application.Adapters.WalletServiceAdapter>();
        services.AddScoped<Application.Adapters.IConfigurationServiceAdapter, Application.Adapters.ConfigurationServiceAdapter>();
    }
}