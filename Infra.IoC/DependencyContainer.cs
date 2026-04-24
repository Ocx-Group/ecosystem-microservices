using Ecosystem.Domain.Core.Bus;
using Ecosystem.Infra.IoC.Storage;
using InfraBus = Ecosystem.Infra.Bus.MassTransitBus;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecosystem.Infra.IoC;

public static class DependencyContainer
{
    public static void AddInfrastructure(
        this IServiceCollection services,
        string rabbitMqHost,
        string username = "guest",
        string password = "guest")
    {
        services.AddMassTransit(x =>
        {
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

        services.AddScoped<IEventBus, InfraBus>();
    }

    public static void AddObjectStorage(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ObjectStorageSettings>(configuration.GetSection(ObjectStorageSettings.SectionName));
        services.AddScoped<IObjectStorageService, SpacesObjectStorageService>();
    }
}
