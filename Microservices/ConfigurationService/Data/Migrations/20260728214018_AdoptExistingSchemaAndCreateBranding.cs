using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecosystem.ConfigurationService.Data.Migrations;

/// <summary>
/// Adopts the pre-existing ConfigurationService schema into EF migrations and
/// installs the new brand configuration and PDF template structures.
///
/// Every statement is idempotent because production may already contain the
/// tables from the original manual seed scripts.
/// </summary>
public partial class AdoptExistingSchemaAndCreateBranding : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(BrandingMigrationSql.BrandConfigurationAndSeed);
        migrationBuilder.Sql(BrandingMigrationSql.PdfTemplatesAndSeed);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        // This migration adopts tables that may predate EF migration history.
        // Dropping them automatically could destroy production data.
        throw new NotSupportedException(
            "The branding adoption migration is irreversible. Restore from a database backup if rollback is required.");
    }
}
