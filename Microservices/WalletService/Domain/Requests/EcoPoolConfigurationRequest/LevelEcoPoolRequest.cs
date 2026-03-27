namespace Ecosystem.WalletService.Domain.Requests.EcoPoolConfigurationRequest;

public class LevelEcoPoolRequest
{
    public int Id { get; set; }
    public int Level { get; set; }
    public decimal Percentage { get; set; }
}