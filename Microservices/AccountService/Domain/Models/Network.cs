namespace Ecosystem.AccountService.Domain.Models;

public partial class Network
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public bool? Status { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<AffiliatesBtc> AffiliatesBtcs { get; } = new List<AffiliatesBtc>();
}
