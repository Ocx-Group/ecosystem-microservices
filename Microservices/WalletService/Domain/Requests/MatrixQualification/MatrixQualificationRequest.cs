namespace Ecosystem.WalletService.Domain.Requests.MatrixQualification;

public class MatrixQualificationRequest
{
    public int QualificationId { get; set; }
    public long UserId { get; set; }
    public int MatrixType { get; set; }
    public decimal TotalEarnings { get; set; }
    public decimal WithdrawnAmount { get; set; }
    public decimal AvailableBalance { get; set; }
    public bool IsQualified { get; set; }
    public DateTime QualificationDate { get; set; }
}