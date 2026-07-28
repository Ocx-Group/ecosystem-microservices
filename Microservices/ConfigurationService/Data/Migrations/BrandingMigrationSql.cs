using System.Reflection;

namespace Ecosystem.ConfigurationService.Data.Migrations;

internal static class BrandingMigrationSql
{
    private const string ResourcePrefix =
        "Ecosystem.ConfigurationService.Data.Migrations.Sql.";

    public static string BrandConfigurationAndSeed { get; } =
        ReadEmbeddedSql("seed_brand_configuration.sql");

    public static string PdfTemplatesAndSeed { get; } =
        ReadEmbeddedSql("seed_pdf_templates.sql");

    private static string ReadEmbeddedSql(string fileName)
    {
        var resourceName = ResourcePrefix + fileName;
        var assembly = typeof(BrandingMigrationSql).Assembly;

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException(
                $"Embedded migration SQL resource '{resourceName}' was not found.");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
