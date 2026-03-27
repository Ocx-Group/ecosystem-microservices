namespace Ecosystem.WalletService.Domain.Requests.MatrixEarningRequest;

public class MatrixEarningRequest
{
    public int EarningId { get; set; }
    public long UserId { get; set; }
    public int MatrixType { get; set; }
    public decimal Amount { get; set; }
    public int SourceUserId { get; set; }
    public string EarningType { get; set; } = null!;
}