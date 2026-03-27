namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public record DebitTransactionRequest
{
    public int AffiliateId { get; init; }
    public int UserId { get; init; }
    public string Concept { get; init; } = string.Empty;
    public decimal Points { get; init; }
    public decimal Commissionable { get; init; }
    public string PaymentMethod { get; init; } = string.Empty;
    public short Origin { get; init; }
    public int Level { get; init; }
    public decimal Debit { get; init; }
    public string AffiliateUserName { get; init; } = string.Empty;
    public string AdminUserName { get; init; } = string.Empty;
    public string ConceptType { get; init; } = string.Empty;
    public long BrandId { get; init; }
    public string? Bank { get; init; }
    public string? ReceiptNumber { get; init; }
    public bool Type { get; init; }
    public string? SecretKey { get; init; }
    public string? Reason { get; init; }
    public bool? IncludeInCommissionCalculation { get; set; }
    public List<InvoiceDetailsTransactionRequest>? invoices { get; init; } = new List<InvoiceDetailsTransactionRequest>();
}