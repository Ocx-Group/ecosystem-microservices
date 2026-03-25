namespace Ecosystem.AccountService.Domain.Models;

public partial class AffiliatesCountRegister
{
    public long Id { get; set; }
    public long AffiliateId { get; set; }
    public int? Year { get; set; }
    public int? Month { get; set; }
    public int? Count { get; set; }
    public int? CountRedBinaria { get; set; }
    public int? CountRedForzada { get; set; }
    public virtual UsersAffiliate Affiliate { get; set; } = null!;
}
