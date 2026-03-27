
namespace Ecosystem.WalletService.Domain.Models;

public partial class ResultsModel2
{
    public long Id { get; set; }

    public long EcopoolConfigurationId { get; set; }

    public int ProductExternalId { get; set; }

    public int AffiliateId { get; set; }

    public string AffiliateName { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public DateTime PaymentDate { get; set; }

    public DateTime LastDayDate { get; set; }

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

    public string Points { get; set; } = null!;

    public string CasePool { get; set; } = null!;

    public DateOnly PeriodPool { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime? UserCreatedAt { get; set; }

    public virtual ModelConfiguration ModelConfiguration { get; set; } = null!;

    public virtual ICollection<ResultsModel2Level> ResultsModel2Levels { get; } = new List<ResultsModel2Level>();
}
