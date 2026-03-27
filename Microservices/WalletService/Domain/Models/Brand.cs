
namespace Ecosystem.WalletService.Domain.Models;

public partial class Brand
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string SecretKey { get; set; } = null!;

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Transaction> CoinpaymentTransactions { get; } = new List<Transaction>();

    public virtual ICollection<Invoice> Invoices { get; } = new List<Invoice>();

    public virtual ICollection<InvoicesDetail> InvoicesDetails { get; } = new List<InvoicesDetail>();

    public virtual ICollection<Wallet> Wallets { get; } = new List<Wallet>();

    public virtual ICollection<WalletsRequest> WalletsRequests { get; } = new List<WalletsRequest>();

    public virtual ICollection<WalletsServiceModel2> WalletsServiceModel2s { get; } = new List<WalletsServiceModel2>();
}
