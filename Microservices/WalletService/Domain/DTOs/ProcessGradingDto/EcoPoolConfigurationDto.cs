namespace Ecosystem.WalletService.Domain.DTOs.ProcessGradingDto;

public class ModelConfigurationDto
{
    public int Id { get; set; }
    public double CompanyPercentage { get; set; }
    public double CompanyPercentageLevels { get; set; }
    public double ModelPercentage { get; set; }
    public double MaxGainLimit { get; set; }
    public DateTime DateInit { get; set; }
    public DateTime DateEnd { get; set; }
    public int Case { get; set; }
    public int? Processed { get; set; }
    public int? Totals { get; set; }
    
    public string ModelType { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public ICollection<EcoPoolLevelDto> ModelConfigurationLevels { get; set; } = new List<EcoPoolLevelDto>();
}