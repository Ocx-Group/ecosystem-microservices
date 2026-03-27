namespace Ecosystem.WalletService.Domain.DTOs.InvoiceDto;

public class InvoiceTradingAcademyDto
{
    public int      ProductId    { get; set; }
    public string   UserName     { get; set; } = string.Empty;
    public int      InvoiceId    { get; set; }
    public string   ProductName  { get; set; } = string.Empty;
    public decimal  ProductPrice { get; set; }
    public DateTime CreatedAt    { get; set; }
    public string   StartDay     { get; set; } = string.Empty;
    public string   EndDay       { get; set; } = string.Empty;
}