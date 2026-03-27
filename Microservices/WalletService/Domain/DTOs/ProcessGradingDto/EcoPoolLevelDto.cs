namespace Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;

public class EcoPoolLevelDto
{
    public int Id { get; set; }
    public int EcoPoolConfigurationId { get; set; }
    public int Level { get; set; }
    public double Percentage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}