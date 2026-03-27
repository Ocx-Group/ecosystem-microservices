namespace Ecosystem.WalletService.Data.Types;

[NpgsqlTypes.PgName("invoices_details_type_with_brand")]
public class InvoiceDetailTypeWithBrand
{
    public int product_id { get; set; }
    public int payment_group_id { get; set; }
    public bool accummin_purchase { get; set; }
    public string product_name { get; set; }
    public decimal product_price { get; set; }
    public decimal product_price_btc { get; set; }
    public decimal product_iva { get; set; }
    public int product_quantity { get; set; }
    public decimal product_commissionable { get; set; }
    public decimal binary_points { get; set; }
    public decimal product_points { get; set; }
    public decimal product_discount { get; set; }
    public int combination_id { get; set; }
    public bool product_pack { get; set; }
    public decimal base_amount { get; set; }
    public decimal daily_percentage { get; set; }
    public int waiting_days { get; set; }
    public int days_to_pay_quantity { get; set; }
    public bool product_start { get; set; }
    public int brand_id { get; set; }
}