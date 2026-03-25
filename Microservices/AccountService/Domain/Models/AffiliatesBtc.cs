namespace Ecosystem.AccountService.Domain.Models;

public partial class AffiliatesBtc
{
    public long Id { get; set; }
    public long AffiliateId { get; set; }
    public string Address { get; set; } = null!;
    public short Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? BrandId { get; set; }
    public long? NetworkId { get; set; }
    public virtual UsersAffiliate Affiliate { get; set; } = null!;
    public virtual Brand? Brand { get; set; }
    public virtual Network? Network { get; set; }
}
