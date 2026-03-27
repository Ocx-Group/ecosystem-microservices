namespace Ecosystem.WalletService.Domain.Requests.WalletPayRequest;

public class WalletPayRequest
{
    public WalletRequest.WalletRequest WalletPay { get; set; }
    public InvoiceRequest.InvoiceRequest Invoice { get; set; }
}