namespace Ecosystem.AccountService.Domain.Models;

public partial class Brand
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<AffiliatesBtc> AffiliatesBtcs { get; } = new List<AffiliatesBtc>();
    public virtual ICollection<LoginMovement> LoginMovements { get; } = new List<LoginMovement>();
    public virtual ICollection<Ticket> Tickets { get; } = new List<Ticket>();
    public virtual ICollection<User> Users { get; } = new List<User>();
    public virtual ICollection<UsersAffiliate> UsersAffiliates { get; } = new List<UsersAffiliate>();
}
