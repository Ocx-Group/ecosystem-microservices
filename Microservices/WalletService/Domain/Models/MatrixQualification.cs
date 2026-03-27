namespace Ecosystem.WalletService.Domain.Models;

public class MatrixQualification
{
    public int QualificationId { get; set; }
    public long UserId { get; set; }
    public int MatrixType { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal WithdrawnAmount { get; set; }
    public decimal AvailableBalance { get; set; }
    public bool IsQualified { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public int QualificationCount { get; set; }
    public decimal LastQualificationTotalEarnings { get; set; }
    public decimal LastQualificationWithdrawnAmount { get; set; }
    public DateTime? LastQualificationDate { get; set; }
}