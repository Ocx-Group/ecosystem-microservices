namespace Ecosystem.WalletService.Domain.CustomModels;

public class InvoiceModelOneAndTwoResponse
{
    public string   UserName       { get; set; } = string.Empty;
    public int      InvoiceId      { get; set; }
    public string   ProductName    { get; set; } = string.Empty;
    public decimal  BaseAmount   { get; set; }
    public int      PaymentGroupId { get; set; }
    public DateTime CreatedAt      { get; set; }
}