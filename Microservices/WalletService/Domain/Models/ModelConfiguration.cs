
namespace Ecosystem.WalletService.Domain.Models;

public partial class ModelConfiguration
{
    public long Id { get; set; }

    public decimal CompanyPercentage { get; set; }

    public decimal ModelPercentage { get; set; }

    public decimal MaxGainLimit { get; set; }

    public DateTime DateInit { get; set; }

    public DateTime DateEnd { get; set; }

    public int Case { get; set; }

    public DateTime? CompletedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public decimal CompanyPercentageLevels { get; set; }

    public int? Processed { get; set; }

    public int? Totals { get; set; }

    public string ModelType { get; set; } = null!;

    public virtual ICollection<ModelConfigurationLevel> ModelConfigurationLevels { get; } = new List<ModelConfigurationLevel>();

    public virtual ICollection<ResultsModel2> ResultsModel2s { get; } = new List<ResultsModel2>();
}
