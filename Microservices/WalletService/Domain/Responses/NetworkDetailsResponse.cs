namespace Ecosystem.WalletService.Domain.Responses;

public class NetworkDetailsResponse
{
    public StatisticsModel12356Dto Model123 { get; set; } 
    public StatisticsModel4Dto Model4 { get; set; } 
    public StatisticsModel12356Dto Model5 { get; set; } 
    public StatisticsModel12356Dto Model6 { get; set; }
}

public class StatisticsModel12356Dto
{
    public int DirectAffiliates { get; set; }
    public int IndirectAffiliates { get; set; }
}

public class StatisticsModel4Dto
{
    public int LeftCount { get; set; }
    public int RightCount { get; set; }
}