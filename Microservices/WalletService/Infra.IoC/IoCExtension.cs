using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Mappings;
using Ecosystem.WalletService.Application.Services;
using Ecosystem.WalletService.Application.Strategies;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Data.Repositories;
using Ecosystem.WalletService.Data.UnitOfWork;
using Ecosystem.WalletService.Domain.Configuration;
using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Services;
using Ecosystem.Grpc.Account;
using Ecosystem.Grpc.Configuration;
using Ecosystem.Grpc.Inventory;
using Ecosystem.Infra.IoC.MultiTenancy;
using FluentValidation;
using Grpc.Core;
using Grpc.Net.Client.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BrandTenantStore = Ecosystem.WalletService.Data.Repositories.BrandTenantStore;
using ApiClientTokenValidator = Ecosystem.WalletService.Data.Repositories.ApiClientTokenValidator;

namespace Ecosystem.WalletService.Infra.IoC;

public static class IoCExtension
{
    public static void AddWalletServiceDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApplicationConfiguration>(configuration.GetSection("AppSettings"));

        services.AddWalletServiceDbContext(configuration);
        services.AddMultiTenancy<BrandTenantStore, ApiClientTokenValidator>();
        services.InjectAutoMapper();
        services.InjectMediatR();
        services.InjectValidators();
        services.InjectRepositories();
        services.InjectDomainServices();
        services.InjectGrpcClients(configuration);
        services.InjectPaymentGatewayClients(configuration);
    }

    private static void InjectPaymentGatewayClients(this IServiceCollection services, IConfiguration configuration)
    {
        var coinPayUrl = configuration["AppSettings:Endpoints:CoinPayURL"] ?? "https://api.coinpay.cr";

        // Named client used by the token provider, which is a singleton and therefore
        // must not hold a typed HttpClient of its own.
        services.AddHttpClient(CoinPayConstants.HttpClientName, client =>
        {
            client.BaseAddress = new Uri(coinPayUrl);
        });

        services.AddSingleton<ICoinPayTokenProvider, CoinPayTokenProvider>();

        services.AddHttpClient<ICoinPayAdapter, CoinPayAdapter>(client =>
        {
            client.BaseAddress = new Uri(coinPayUrl);
        });

        var conPaymentsUrl = configuration["AppSettings:Endpoints:ConPaymentsURL"]
                             ?? "https://www.coinpayments.net";

        services.AddHttpClient<ICoinPaymentsAdapter, CoinPaymentsAdapter>(client =>
        {
            client.BaseAddress = new Uri(conPaymentsUrl);
        });
    }

    private static void InjectDomainServices(this IServiceCollection services)
    {
        // Payment domain services
        services.AddScoped<IProductValidationService, ProductValidationService>();
        services.AddScoped<IPaymentCalculator, PaymentCalculator>();
        services.AddScoped<IInvoiceDetailFactory, InvoiceDetailFactory>();
        services.AddScoped<IDebitTransactionBuilder, DebitTransactionBuilder>();
        services.AddScoped<IBalanceValidationService, BalanceValidationService>();
        services.AddScoped<IPaymentNotificationService, PaymentNotificationService>();
        services.AddScoped<IMembershipBonusService, MembershipBonusService>();
        services.AddScoped<IPurchaseBonusService, PurchaseBonusService>();

        // Payment strategy
        services.AddScoped<IBalancePaymentStrategy, BalancePaymentStrategy>();
        services.AddScoped<IExternalPaymentStrategy, ExternalPaymentStrategy>();

        // PDF generation (browser singleton for Chromium reuse)
        services.AddSingleton<IBrowserProvider, BrowserProvider>();
        services.AddScoped<IPdfService, PdfService>();
    }

    private static void InjectRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IApiClientRepository, ApiClientRepository>();
        services.AddScoped<IBonusRepository, BonusRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<ICreditRepository, CreditRepository>();
        services.AddScoped<IEcoPoolConfigurationRepository, EcoPoolConfigurationRepository>();
        services.AddScoped<IInvoiceDetailRepository, InvoiceDetailRepository>();
        services.AddScoped<IInvoiceRepository, InvoiceRepository>();
        services.AddScoped<IMatrixEarningsRepository, MatrixEarningsRepository>();
        services.AddScoped<IMatrixQualificationRepository, MatrixQualificationRepository>();
        services.AddScoped<INetworkPurchaseRepository, NetworkPurchaseRepository>();
        services.AddScoped<IResultsEcoPoolRepository, ResultsEcoPoolRepository>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IWalletHistoryRepository, WalletHistoryRepository>();
        services.AddScoped<IWalletModel1ARepository, WalletModel1ARepository>();
        services.AddScoped<IWalletModel1BRepository, WalletModel1BRepository>();
        services.AddScoped<IWalletPeriodRepository, WalletPeriodRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IWalletRequestRepository, WalletRequestRepository>();
        services.AddScoped<IWalletRetentionConfigRepository, WalletRetentionConfigRepository>();
        services.AddScoped<IWalletWaitRepository, WalletWaitRepository>();
        services.AddScoped<IWalletWithDrawalRepository, WalletWithDrawalRepository>();
    }

    private static void InjectMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(WalletMappingProfile).Assembly));
    }

    private static void InjectValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(WalletMappingProfile).Assembly);
    }

    private static void InjectAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(WalletMappingProfile).Assembly);
    }

    private static void AddWalletServiceDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgreSqlConnection");
        services.AddDbContext<WalletServiceDbContext>(options =>
            options.UseNpgsql(connectionString));

        // UnitOfWork takes the base DbContext, which AddDbContext does not register on its own.
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<WalletServiceDbContext>());
    }

    private static void InjectGrpcClients(this IServiceCollection services, IConfiguration configuration)
    {
        var inventoryServiceUrl = configuration["GrpcServices:InventoryService"] ?? "https://localhost:5101";

        services.AddGrpcClient<InventoryGrpc.InventoryGrpcClient>(o =>
        {
            o.Address = new Uri(inventoryServiceUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(CreateResilientGrpcHandler);

        services.AddScoped<IInventoryServiceAdapter, GrpcInventoryServiceAdapter>();

        var accountServiceUrl = configuration["GrpcServices:AccountService"] ?? "https://localhost:5201";
        services.AddGrpcClient<AccountGrpc.AccountGrpcClient>(o =>
        {
            o.Address = new Uri(accountServiceUrl);
        })
        .ConfigurePrimaryHttpMessageHandler(CreateResilientGrpcHandler);
        services.AddScoped<IAccountServiceAdapter, GrpcAccountServiceAdapter>();

        var configurationServiceUrl = configuration["GrpcServices:ConfigurationService"] ?? "https://localhost:5301";
        services.AddGrpcClient<ConfigurationGrpc.ConfigurationGrpcClient>(o =>
        {
            o.Address = new Uri(configurationServiceUrl);

            // Only this client retries. Every method on ConfigurationGrpc is a read, so a
            // second attempt cannot double-apply anything; the Account and Inventory
            // clients carry operations that must not be replayed blindly.
            o.ChannelOptionsActions.Add(
                channel => channel.ServiceConfig = ReadOnlyRetryServiceConfig());
        })
        .ConfigurePrimaryHttpMessageHandler(CreateResilientGrpcHandler);
        services.AddScoped<IConfigurationAdapter, GrpcConfigurationAdapter>();
    }

    /// <summary>
    /// gRPC keeps one long-lived HTTP/2 connection per sibling service in the pool. With
    /// the default handler that connection outlives the other end: Kestrel closes an idle
    /// HTTP/2 connection after 130 seconds, and the client finds out only when it tries to
    /// send on it — surfacing as an intermittent "Exception while reading from stream"
    /// against a service that is perfectly healthy.
    ///
    /// Retiring pooled connections well before that window, and pinging the ones that stay
    /// open, keeps a request from ever landing on a dead connection.
    /// </summary>
    private static SocketsHttpHandler CreateResilientGrpcHandler() => new()
    {
        PooledConnectionIdleTimeout = TimeSpan.FromSeconds(60),
        KeepAlivePingDelay = TimeSpan.FromSeconds(50),
        KeepAlivePingTimeout = TimeSpan.FromSeconds(20),
        KeepAlivePingPolicy = HttpKeepAlivePingPolicy.Always,
        EnableMultipleHttp2Connections = true
    };

    /// <summary>
    /// Transparent retry for the read-only configuration calls, so a connection that dies
    /// anyway — a rolling deploy of ConfigurationService, a dropped idle connection — costs
    /// a few hundred milliseconds instead of a 500 in the caller's face.
    ///
    /// <c>Unavailable</c> is the connection that never opened; <c>Internal</c> is the one
    /// that broke mid-response, which is the shape this failure takes.
    /// </summary>
    private static ServiceConfig ReadOnlyRetryServiceConfig() => new()
    {
        MethodConfigs =
        {
            new MethodConfig
            {
                Names = { MethodName.Default },
                RetryPolicy = new RetryPolicy
                {
                    MaxAttempts = 3,
                    InitialBackoff = TimeSpan.FromMilliseconds(200),
                    MaxBackoff = TimeSpan.FromSeconds(2),
                    BackoffMultiplier = 2,
                    RetryableStatusCodes = { StatusCode.Unavailable, StatusCode.Internal }
                }
            }
        }
    };
}
