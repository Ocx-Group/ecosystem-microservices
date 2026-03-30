using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Mappings;
using Ecosystem.WalletService.Application.Services;
using Ecosystem.WalletService.Application.Strategies;
using Ecosystem.WalletService.Data.Context;
using Ecosystem.WalletService.Data.Repositories;
using Ecosystem.WalletService.Data.UnitOfWork;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Services;
using Ecosystem.Infra.IoC.MultiTenancy;
using FluentValidation;
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
        services.AddWalletServiceDbContext(configuration);
        services.AddMultiTenancy<BrandTenantStore, ApiClientTokenValidator>();
        services.InjectAutoMapper();
        services.InjectMediatR();
        services.InjectValidators();
        services.InjectRepositories();
        services.InjectDomainServices();
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

        // Payment strategy
        services.AddScoped<IBalancePaymentStrategy, BalancePaymentStrategy>();

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
    }
}
