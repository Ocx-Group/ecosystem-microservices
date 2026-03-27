using System.ComponentModel.DataAnnotations.Schema;

namespace Ecosystem.WalletService.Domain.CustomModels;

public class PurchasesPerMonth
{
    [Column("year")] 
    public int Year { get; set; }

    [Column("month")] 
    public int Month { get; set; }

    [Column("total_purchases")] 
    public int TotalPurchases { get; set; }
}