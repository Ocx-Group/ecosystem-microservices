using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecosystem.ConfigurationService.Data.Migrations
{
    /// <summary>
    /// Adds the per-brand switch that decides whether the purchase bonus is paid on
    /// every purchase or only on the ones that opted in via
    /// <c>WalletRequest.DailyBonusActivation</c>.
    ///
    /// New rows default to <c>false</c>, but existing rows with the commission already
    /// enabled are backfilled to <c>true</c>: until now those brands distributed on
    /// every external purchase with no opt-in check, and defaulting them to
    /// <c>false</c> would silently stop payouts that are live today.
    /// </summary>
    public partial class AddDailyBonusAlwaysDistribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "daily_bonus_always_distribute",
                schema: "configuration_service",
                table: "brand_configuration",
                type: "boolean",
                nullable: false,
                defaultValueSql: "false");

            migrationBuilder.Sql(
                """
                UPDATE configuration_service.brand_configuration
                SET daily_bonus_always_distribute = true
                WHERE commission_enabled = true;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "daily_bonus_always_distribute",
                schema: "configuration_service",
                table: "brand_configuration");
        }
    }
}
