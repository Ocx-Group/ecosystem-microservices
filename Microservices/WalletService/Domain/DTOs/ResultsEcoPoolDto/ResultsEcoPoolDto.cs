namespace Ecosystem.WalletService.Domain.DTOs.ResultsEcoPoolDto;

public class ResultsEcoPoolDto
{
    public int Id { get; set; }
    public int EcoPoolConfigurationId { get; set; }
    public int ProductExternalId { get; set; }
    public int AffiliateId { get; set; }
    public string AffiliateName { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public DateTime LastDaydate { get; set; }
    public decimal DailyPercentage { get; set; }
    public decimal BasePack { get; set; }
    public int DaysAmount { get; set; }
    public decimal BaseAmount { get; set; }
    public decimal CompanyAmount { get; set; }
    public decimal CompanyPercentage { get; set; }
    public decimal ProfitDistributedLevels { get; set; }
    public decimal TotalPercentage { get; set; }
    public decimal DeductionAmount { get; set; }
    public decimal PaymentAmount { get; set; }
    public string Points { get; set; } = string.Empty;
    public string CasePool { get; set; } = string.Empty;
    public DateTime PeriodPool { get; set; }

    public virtual ICollection<ResultEcoPoolLevelsDto.ResultEcoPoolLevelsDto> ResultEcoPoolLevels { get; set; }
}