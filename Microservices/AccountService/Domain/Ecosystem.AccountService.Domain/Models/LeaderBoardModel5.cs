namespace Ecosystem.AccountService.Domain.Models;

public partial class LeaderBoardModel5
{
    public long Id { get; set; }
    public int AffiliateId { get; set; }
    public int? FatherModel5 { get; set; }
    public int Level { get; set; }
    public DateTime UserCreatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Amount { get; set; }
    public string Username { get; set; } = null!;
    public int GradingId { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
}
