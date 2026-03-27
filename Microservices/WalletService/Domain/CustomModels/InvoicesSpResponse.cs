using System.ComponentModel.DataAnnotations.Schema;

namespace Ecosystem.WalletService.Domain.CustomModels;

public class InvoicesSpResponse
{
    [Column("id")]
    public int Id { get; set; }
    
    [Column("invoice_number")]
    public int InvoiceNumber { get; set; }
    
    [Column("purchase_order_id")]
    public int? PurchaseOrderId { get; set; }
    
    [Column("affiliate_id")]
    public int AffiliateId { get; set; }
    
    [Column("total_invoice")]
    public decimal? TotalInvoice { get; set; }
    
    [Column("total_invoice_btc")]
    public decimal TotalInvoiceBtc { get; set; }
    
    [Column("total_commissionable")]
    public decimal? TotalCommissionable { get; set; }
    
    [Column("total_points")]
    public int? TotalPoints { get; set; }
    
    [Column("state")]
    public bool State { get; set; }
    
    [Column("status")]
    public bool Status { get; set; }
    
    [Column("date")]
    public DateTime? Date { get; set; }
    
    [Column("cancellation_date")]
    public DateTime? CancellationDate { get; set; }
    
    [Column("payment_method")]
    public string? PaymentMethod { get; set; }
    
    [Column("bank")]
    public string? Bank { get; set; }
    
    [Column("receipt_number")]
    public string? ReceiptNumber { get; set; }
    
    [Column("deposit_date")]
    public DateTime? DepositDate { get; set; }
    
    [Column("type")]
    public bool? Type { get; set; }
    
    [Column("reason")]
    public string? Reason { get; set; }
    
    [Column("invoice_data")]
    public string? InvoiceData { get; set; }
    
    [Column("invoice_address")]
    public string? InvoiceAddress { get; set; }
    
    [Column("shipping_address")]
    public string? ShippingAddress { get; set; }
    
    [Column("secret_key")]
    public string? SecretKey { get; set; }
    
    [Column("btc_address")]
    public string? BtcAddress { get; set; }
    
    [Column("recurring")]
    public int Recurring { get; set; }
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; }
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    [Column("deleted_at")]
    public DateTime? DeletedAt { get; set; }
    
    [Column("brand_id")]
    public long BrandId { get; set; }
}