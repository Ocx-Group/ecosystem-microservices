
namespace Ecosystem.WalletService.Domain.Models;

public partial class TransactionType
{
    public int TransactionTypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<BonusTransactionHistory> BonusTransactionHistories { get; } = new List<BonusTransactionHistory>();
}
