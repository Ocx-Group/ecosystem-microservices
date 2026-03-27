namespace Ecosystem.WalletService.Domain.DTOs.InvoiceDto;

public class ModelBalancesAndInvoicesDto
{
    public string  UserName      { get; set; } = string.Empty;
    public decimal Model1AAmount { get; set; }
    public decimal Model1BAmount { get; set; }
    public decimal Model2Amount  { get; set; }
    public long[]   InvoiceId     { get; set; } = Array.Empty<long>();
}