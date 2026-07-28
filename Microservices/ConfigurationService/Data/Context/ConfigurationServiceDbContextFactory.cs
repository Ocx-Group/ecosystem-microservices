using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ecosystem.ConfigurationService.Data.Context;

/// <summary>
/// Creates the ConfigurationService context for EF Core design-time commands.
/// The connection is only metadata for migration scaffolding; no database
/// connection is opened while migrations are generated.
/// </summary>
public sealed class ConfigurationServiceDbContextFactory
    : IDesignTimeDbContextFactory<ConfigurationServiceDbContext>
{
    public ConfigurationServiceDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ConfigurationServiceDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=5432;Database=ecosystem_design;Username=postgres;Password=postgres")
            .Options;

        return new ConfigurationServiceDbContext(options);
    }
}
