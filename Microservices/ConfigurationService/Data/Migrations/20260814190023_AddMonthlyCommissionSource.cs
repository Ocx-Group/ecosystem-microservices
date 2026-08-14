using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecosystem.ConfigurationService.Data.Migrations
{
    /// <summary>
    /// Picks which stored procedure liquidates each brand's monthly commission.
    ///
    /// Every brand defaults to <c>PaymentGroup</c>, the procedure that has always been
    /// used, so Ecosystem, RecyCoin and HouseCoin are unaffected. RecyBot is switched to
    /// <c>InvoiceTotal</c>: its invoices were created by the RecyCoin -> RecyBot data
    /// migration (payment_method = 'migration_brand2_to_brand5') with no rows in
    /// invoices_details at all, and the payment-group procedure reaches an invoice only
    /// through its details, so it finds none of them whatever payment group is asked for.
    /// </summary>
    public partial class AddMonthlyCommissionSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "monthly_commission_source",
                schema: "configuration_service",
                table: "brand_configuration",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValueSql: "'PaymentGroup'");

            // Brand 5 is RecyBot. Written as a plain UPDATE rather than left to an
            // administrator because no dashboard exposes this field yet, and a brand
            // left on the default silently liquidates nothing.
            migrationBuilder.Sql("""
                UPDATE configuration_service.brand_configuration
                SET monthly_commission_source = 'InvoiceTotal'
                WHERE brand_id = 5;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "monthly_commission_source",
                schema: "configuration_service",
                table: "brand_configuration");
        }
    }
}
