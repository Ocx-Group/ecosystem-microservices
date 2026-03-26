namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class BankDetail
{
    public long Id { get; set; }
    public long BankId { get; set; }
    public string Api { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual Bank Bank { get; set; } = null!;
}
