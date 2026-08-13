using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecosystem.ConfigurationService.Data.Migrations
{
    /// <summary>
    /// Adds the per-brand parameters of the monthly commission liquidation, which
    /// until now lived nowhere: <c>calculate_monthly_commissions</c> existed only in
    /// the database and was invoked by hand with whatever values the operator typed.
    ///
    /// Each brand liquidates the payment group of its own product, at its own rate
    /// and with its own waiting days, so these cannot be constants.
    ///
    /// Unlike <see cref="AddDailyBonusAlwaysDistribute"/>, nothing is backfilled to an
    /// active state: no brand was liquidating automatically before this migration, so
    /// enabling any of them here would start paying money that nobody has reviewed.
    /// The payment group is seeded from <c>default_payment_group_id</c> purely as a
    /// starting suggestion for the administrator, and the feature stays off until it
    /// is turned on from admin/calculate-commissions.
    /// </summary>
    public partial class AddMonthlyCommissionSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "monthly_commission_enabled",
                schema: "configuration_service",
                table: "brand_configuration",
                type: "boolean",
                nullable: false,
                defaultValueSql: "false");

            migrationBuilder.AddColumn<decimal>(
                name: "monthly_commission_interest_rate",
                schema: "configuration_service",
                table: "brand_configuration",
                type: "numeric(5,2)",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.AddColumn<int>(
                name: "monthly_commission_payment_group_id",
                schema: "configuration_service",
                table: "brand_configuration",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "monthly_commission_waiting_days",
                schema: "configuration_service",
                table: "brand_configuration",
                type: "integer",
                nullable: false,
                defaultValueSql: "0");

            migrationBuilder.Sql(
                """
                UPDATE configuration_service.brand_configuration
                SET monthly_commission_payment_group_id = default_payment_group_id
                WHERE default_payment_group_id IS NOT NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "monthly_commission_enabled",
                schema: "configuration_service",
                table: "brand_configuration");

            migrationBuilder.DropColumn(
                name: "monthly_commission_interest_rate",
                schema: "configuration_service",
                table: "brand_configuration");

            migrationBuilder.DropColumn(
                name: "monthly_commission_payment_group_id",
                schema: "configuration_service",
                table: "brand_configuration");

            migrationBuilder.DropColumn(
                name: "monthly_commission_waiting_days",
                schema: "configuration_service",
                table: "brand_configuration");
        }
    }
}
