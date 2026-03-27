
namespace Ecosystem.WalletService.Domain.Models;

public partial class InvoicesDetail
{
    public long Id { get; set; }

    public long InvoiceId { get; set; }

    public int ProductId { get; set; }

    public int PaymentGroupId { get; set; }

    public bool AccumminPurchase { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public decimal? ProductPriceBtc { get; set; }

    public decimal? ProductIva { get; set; }

    public int ProductQuantity { get; set; }

    public decimal? ProductCommissionable { get; set; }

    public decimal? BinaryPoints { get; set; }

    public int? ProductPoints { get; set; }

    public decimal? ProductDiscount { get; set; }

    public DateTime Date { get; set; }

    public int? CombinationId { get; set; }

    public bool ProductPack { get; set; }

    public decimal? BaseAmount { get; set; }

    public decimal? DailyPercentage { get; set; }

    public int? WaitingDays { get; set; }

    public int? DaysToPayQuantity { get; set; }

    public bool ProductStart { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long BrandId { get; set; }

    public virtual Brand Brand { get; set; } = null!;

    public virtual Invoice Invoice { get; set; } = null!;
}
