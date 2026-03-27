namespace Ecosystem.WalletService.Domain.Requests.EcoPoolConfigurationRequest;

public class EcoPoolConfigurationRequest
{
    public int? Id { get; set; }
    public decimal CompanyPercentage { get; set; }
    
    public decimal CompanyPercentageLevels { get; set; }
    public decimal EcoPoolPercentage { get; set; }
    public decimal MaxGainLimit { get; set; }
    public DateTime DateInit { get; set; }
    public DateTime DateEnd { get; set; }
    public int Case { get; set; }
    
    public int? Processed { get; set; }
    
    public int? Totals { get; set; }
    public ICollection<LevelEcoPoolRequest> Levels { get; set; }
}