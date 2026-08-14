namespace Ecosystem.WalletService.Domain.DTOs.InvoiceDto;

public class MonthlyPurchasesDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalAmount { get; set; }
}
