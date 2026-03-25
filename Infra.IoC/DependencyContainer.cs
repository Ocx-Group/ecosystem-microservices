using Ecosystem.Domain.Core.Bus;
using InfraBus = Ecosystem.Infra.Bus.MassTransitBus;
using MassTransit;
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
        // MassTransit con RabbitMQ
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

        // Registrar IEventBus → MassTransitBus
        services.AddScoped<IEventBus, InfraBus>();
    }
}