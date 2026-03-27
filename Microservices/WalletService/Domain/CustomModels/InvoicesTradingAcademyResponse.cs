namespace Ecosystem.WalletService.Domain.CustomModels;

public class InvoicesTradingAcademyResponse
{
    public int      ProductId  { get; set; }
    public string   UserName   { get; set; } = string.Empty;
    public int      InvoiceId  { get; set; }
    public string   ProductName { get; set; } = string.Empty;
    public decimal  ProductPrice      { get; set; }
    public DateTime CreatedAt     { get; set; }
}