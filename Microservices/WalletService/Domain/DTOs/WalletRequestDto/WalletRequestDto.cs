namespace Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;

public class WalletRequestDto
{
    public int Id { get; set; }
    public int AffiliateId { get; set; }
    public int PaymentAffiliateId { get; set; }
    public string? OrderNumber { get; set; }
    public string? AdminUserName { get; set; }
    public decimal Amount { get; set; }
    public string? Concept { get; set; }
    public string? Type { get; set; }
    public int? InvoiceNumber { get; set; }
    public DateTime CreationDate { get; set; }
    public byte? Status { get; set; }
    public DateTime? AttentionDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}