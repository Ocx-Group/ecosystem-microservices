
namespace Ecosystem.WalletService.Domain.Models;

public partial class ModelConfigurationLevel
{
    public long Id { get; set; }

    public long EcopoolConfigurationId { get; set; }

    public int Level { get; set; }

    public decimal Percentage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ModelConfiguration EcopoolConfiguration { get; set; } = null!;
}
