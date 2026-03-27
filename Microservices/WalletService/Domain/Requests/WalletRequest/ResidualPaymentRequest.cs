namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class ResidualPaymentRequest
{
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
}