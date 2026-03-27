namespace Ecosystem.WalletService.Domain.DTOs.InvoiceDetailDto;

public class InvoiceDTO
{
    public int Id { get; set; }
    public int InvoiceNumber { get; set; }
    public int PurchaseOrderId { get; set; } 
    public int AffiliateId { get; set; }
    public decimal? TotalInvoice { get; set; }
    public decimal TotalInvoiceBtc { get; set; }
    public decimal? TotalCommissionable { get; set; }
    public int? TotalPoints { get; set; }
    public bool State { get; set; }
    public bool Status { get; set; } 
    public DateTime? Date { get; set; }
    public DateTime? CancellationDate { get; set; }
    public string? PaymentMethod { get; set; }
    public string? Bank { get; set; }
    public string? ReceiptNumber { get; set; }
    public DateTime? DepositDate { get; set; }
    public bool? Type { get; set; }
    public string? Reason { get; set; }
    public string? InvoiceData { get; set; }
    public string? InvoiceAddress { get; set;}
    public string? ShippingAddress { get;set;}
    public string? SecretKey{get;set;}
    public string? BtcAddress{get;set;}
    public int Recurring{get;set;}
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

}