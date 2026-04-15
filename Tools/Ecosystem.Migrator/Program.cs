using Ecosystem.Migrator;
using Ecosystem.AccountService.Data.Context;
using Ecosystem.ConfigurationService.Data.Context;
using Ecosystem.InventoryService.Data.Context;
using Ecosystem.WalletService.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

// =============================================================================
// Centralized database migration tool for all Ecosystem microservices.
// Runs as a Kubernetes Job before application deployments to ensure
// all database schemas are up-to-date without race conditions.
//
// Supported services (EF Core + PostgreSQL):
//   - AccountService        (schema: account_service)
//   - ConfigurationService  (schema: configuration_service)
//   - InventoryService      (schema: inventory_service)
//   - WalletService         (schema: wallet_service)
//
// Excluded:
//   - NotificationService   (uses MongoDB — no EF Core migrations)
// =============================================================================
var builder = Host.CreateApplicationBuilder(args);

// Configure logging
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Information);

// Register all DbContexts with their connection strings from environment/secrets
RegisterDbContext<AccountServiceDbContext>(builder, "ACCOUNT_DB_CONNECTION");
RegisterDbContext<ConfigurationServiceDbContext>(builder, "CONFIGURATION_DB_CONNECTION");
RegisterDbContext<InventoryServiceDbContext>(builder, "INVENTORY_DB_CONNECTION");
RegisterDbContext<WalletServiceDbContext>(builder, "WALLET_DB_CONNECTION");

var host = builder.Build();

using var scope = host.Services.CreateScope();
var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

logger.LogInformation("=== Ecosystem Database Migrator ===");
logger.LogInformation("Starting database migrations...");

var exitCode = 0;

try
{
    // Migrate sequentially to avoid race conditions.
    // Order: Account first (other services may reference user/affiliate data).
    var contexts = new (string Name, DbContext Context)[]
    {
        ("AccountService", scope.ServiceProvider.GetRequiredService<AccountServiceDbContext>()),
        ("ConfigurationService", scope.ServiceProvider.GetRequiredService<ConfigurationServiceDbContext>()),
        ("InventoryService", scope.ServiceProvider.GetRequiredService<InventoryServiceDbContext>()),
        ("WalletService", scope.ServiceProvider.GetRequiredService<WalletServiceDbContext>()),
    };

    foreach (var (name, context) in contexts)
    {
        await MigrateContextAsync(name, context, logger);
    }

    logger.LogInformation("=== All migrations completed successfully! ===");
}
catch (Exception ex)
{
    logger.LogCritical(ex, "Migration failed with unrecoverable error.");
    exitCode = 1;
}

return exitCode;

// ============================================================================
// Helper methods
// ============================================================================

// Registers an EF Core DbContext with a PostgreSQL connection string resolved from
// environment variables or appsettings configuration.
static void RegisterDbContext<TContext>(HostApplicationBuilder builder, string connectionStringKey)
    where TContext : DbContext
{
    var connectionString = builder.Configuration[connectionStringKey] ?? builder.Configuration.GetConnectionString(connectionStringKey);

    if (string.IsNullOrEmpty(connectionString))
    {
        throw new InvalidOperationException(
            $"Connection string '{connectionStringKey}' is not configured. " +
            $"Ensure the environment variable or configuration key is set.");
    }

    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
    dataSourceBuilder.EnableDynamicJson();
    var dataSource = dataSourceBuilder.Build();

    builder.Services.AddDbContext<TContext>(options =>
        options.UseNpgsql(dataSource)
            .UseSnakeCaseNamingConvention());
}

// Migrates a single DbContext with retry logic and PostgreSQL-specific error handling.
// Retries on transient failures (connection issues, lock contention).
static async Task MigrateContextAsync(
    string name,
    DbContext context,
    ILogger logger,
    int maxRetries = 5)
{
    logger.LogInformation("[{Context}] Starting migration...", name);

    for (var attempt = 1; attempt <= maxRetries; attempt++)
    {
        var result = await AttemptMigrationAsync(name, context, logger, attempt, maxRetries);

        if (result is MigrationResult.Success or MigrationResult.Skipped)
            return;

        var delaySeconds = result == MigrationResult.LockHeld ? attempt * 3 : attempt * 2;
        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
    }

    throw new InvalidOperationException($"Failed to migrate {name} after {maxRetries} attempts.");
}

// Executes a single migration attempt and returns the outcome.
static async Task<MigrationResult> AttemptMigrationAsync(
    string name,
    DbContext context,
    ILogger logger,
    int attempt,
    int maxRetries)
{
    try
    {
        if (!await context.Database.CanConnectAsync())
        {
            logger.LogWarning("[{Context}] Database not ready (attempt {Attempt}/{Max}). Waiting...",
                name, attempt, maxRetries);
            return MigrationResult.Retry;
        }

        var pending = (await context.Database.GetPendingMigrationsAsync()).ToList();
        if (pending.Count == 0)
        {
            logger.LogInformation("[{Context}] No pending migrations.", name);
            return MigrationResult.Skipped;
        }

        logger.LogInformation("[{Context}] Applying {Count} migrations: {Migrations}",
            name, pending.Count, string.Join(", ", pending));

        await context.Database.MigrateAsync();

        logger.LogInformation("[{Context}] Migration completed successfully.", name);
        return MigrationResult.Success;
    }
    catch (PostgresException ex) when (ex.SqlState == "42P07") // relation already exists
    {
        logger.LogWarning(ex, "[{Context}] Tables already exist (SqlState: 42P07). Skipping.", name);
        return MigrationResult.Skipped;
    }
    catch (PostgresException ex) when (ex.SqlState == "55P03") // lock_not_available
    {
        logger.LogWarning(ex, "[{Context}] Migration lock held (SqlState: 55P03). Retry {Attempt}/{Max}...",
            name, attempt, maxRetries);
        return MigrationResult.LockHeld;
    }
    catch (PostgresException ex) when (ex.SqlState == "23505") // unique_violation
    {
        logger.LogWarning(ex, "[{Context}] Migration already recorded (SqlState: 23505). Skipping.", name);
        return MigrationResult.Skipped;
    }
    catch (Exception ex) when (attempt < maxRetries)
    {
        logger.LogWarning(ex, "[{Context}] Migration failed (attempt {Attempt}/{Max}). Retrying...",
            name, attempt, maxRetries);
        return MigrationResult.Retry;
    }
}
