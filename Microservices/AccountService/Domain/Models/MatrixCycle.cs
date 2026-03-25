namespace Ecosystem.AccountService.Domain.Models;

public class MatrixCycle
{
    public int CycleId { get; set; }
    public int MatrixType { get; set; }
    public int InitiatorUserId { get; set; }
    public int TotalPositions { get; set; }
    public int MaxPositions { get; set; }
    public bool IsCompleted { get; set; }
    public bool RewardPaid { get; set; }
    public DateTime CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
