namespace Ecosystem.WalletService.Domain.Models;

public class MatrixEarning
{
    public int EarningId { get; set; }
    public long UserId { get; set; }
    public int MatrixType { get; set; }
    public decimal Amount { get; set; }
    public int SourceUserId { get; set; }
    public string EarningType { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}