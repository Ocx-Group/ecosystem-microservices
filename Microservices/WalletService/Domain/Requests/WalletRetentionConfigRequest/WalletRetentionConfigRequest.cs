namespace Ecosystem.WalletService.Domain.Requests.WalletRetentionConfigRequest;

public class WalletRetentionConfigRequest
{
    public int Id { get; set; }
    public decimal WithdrawalFrom { get; set; }
    public decimal WithdrawalTo { get; set; }
    public decimal Percentage { get; set; }
    public DateTime Date { get; set; }
    public DateTime? DisableDate { get; set; }
    public bool Status { get; set; }
    
}