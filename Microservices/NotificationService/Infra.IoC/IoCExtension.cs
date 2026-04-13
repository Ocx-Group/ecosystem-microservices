using Ecosystem.Domain.Core.Bus;
using MassTransitBus = Ecosystem.Infra.Bus.MassTransitBus;
using Ecosystem.Grpc.Configuration;
using Ecosystem.NotificationService.Application.Consumers;
using Ecosystem.NotificationService.Application.Mappings;
using Ecosystem.NotificationService.Application.Services;
using Ecosystem.NotificationService.Application.Validators.Template;
using Ecosystem.NotificationService.Data.Context;
using Ecosystem.NotificationService.Data.Repositories;
using Ecosystem.NotificationService.Data.Settings;
using Ecosystem.NotificationService.Domain.Interfaces;
using FluentValidation;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ecosystem.NotificationService.Infra.IoC;

public static class IoCExtension
{
    public static void AddNotificationServiceDependencies(
        this IServiceCollection services,
        IConfiguration configuration,
        string rabbitMqHost,
        string rabbitMqUsername,
        string rabbitMqPassword)
    {
        services.AddMongoDb(configuration);
        services.AddMassTransitWithConsumers(rabbitMqHost, rabbitMqUsername, rabbitMqPassword);
        services.InjectAutoMapper();
        services.InjectMediatR();
        services.InjectValidators();
        services.InjectRepositories();
        services.InjectServices(configuration);
    }

    private static void AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));
        services.AddSingleton<MongoDbContext>();
    }

    private static void AddMassTransitWithConsumers(
        this IServiceCollection services,
        string rabbitMqHost,
        string username,
        string password)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<SendEmailConsumer>();
            x.AddConsumer<PaymentConfirmationConsumer>();
            x.AddConsumer<MembershipConfirmationConsumer>();
            x.AddConsumer<BonusNotificationConsumer>();

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

        services.AddScoped<IEventBus, MassTransitBus>();
    }

    private static void InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
        services.AddScoped<IBrandConfigurationRepository, BrandConfigurationRepository>();
    }

    private static void InjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEmailService>(sp =>
            new BrevoEmailService(
                configuration,
                sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<BrevoEmailService>>()));

        // PDF generation
        services.AddSingleton<IBrowserProvider, Application.Services.Pdf.BrowserProvider>();
        services.AddScoped<IPdfService, Application.Services.Pdf.PdfService>();

        // gRPC client → ConfigurationService for PDF templates
        var configServiceUrl = configuration["GrpcServices:ConfigurationService"] ?? "https://localhost:5103";
        services.AddGrpcClient<ConfigurationGrpc.ConfigurationGrpcClient>(o =>
        {
            o.Address = new Uri(configServiceUrl);
        });
        services.AddScoped<IPdfTemplateProvider, Application.Adapters.GrpcConfigurationServiceAdapter>();
    }

    private static void InjectMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(
                typeof(Application.Commands.Template.CreateTemplateCommand).Assembly));
    }

    private static void InjectValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CreateTemplateValidator).Assembly);
    }

    private static void InjectAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(NotificationMappingProfile).Assembly);
    }
}
