namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class Bank
{
    public long Id { get; set; }
    public string BankName { get; set; } = null!;
    public string KeyName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public short Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<BankDetail> BankDetails { get; } = new List<BankDetail>();
}
