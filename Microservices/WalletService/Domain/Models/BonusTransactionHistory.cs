
namespace Ecosystem.WalletService.Domain.Models;

public partial class BonusTransactionHistory
{
    public long TransactionId { get; set; }

    public long? BonusId { get; set; }

    public long? InvoiceId { get; set; }

    public int? TransactionTypeId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Bonuse? Bonus { get; set; }

    public virtual Invoice? Invoice { get; set; }

    public virtual TransactionType? TransactionType { get; set; }
}
