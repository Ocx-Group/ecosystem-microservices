namespace Ecosystem.WalletService.Domain.DTOs.LeaderBoardDto;

public class LeaderBoardModel4
{
    public int Id { get; set; }
    public int AffiliateId { get; set; }
    public int? FatherModel4 { get; set; }
    public int Level { get; set; }
    public DateTime UserCreatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public decimal Amount { get; set; }
    public decimal Points { get; set; }
    public string UserName { get; set; }
    public int GradingId { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }


}