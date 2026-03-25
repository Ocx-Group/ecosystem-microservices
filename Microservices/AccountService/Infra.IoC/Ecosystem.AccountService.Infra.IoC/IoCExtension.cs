using Ecosystem.AccountService.Application.Interfaces;
using Ecosystem.AccountService.Application.Mappings;
using Ecosystem.AccountService.Application.Validators.Auth;
using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Data.Repositories;
using Ecosystem.AccountService.Domain.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecosystem.AccountService.Infra.IoC;

public static class IoCExtension
{
    public static IServiceCollection AddAccountServiceDependencies(this IServiceCollection services)
    {
        // MediatR (auto-discovers handlers from Application assembly)
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Application.Commands.Auth.UserAuthenticationCommand).Assembly));

        // FluentValidation (auto-discovers validators from Application assembly)
        services.AddValidatorsFromAssembly(typeof(UserAuthenticationValidator).Assembly);

        // AutoMapper profiles
        services.AddAutoMapper(typeof(AuthMappingProfile).Assembly);

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

        return services;
    }

    public static IServiceCollection AddAccountServiceDbContext(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        services.AddDbContext<AccountServiceDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}