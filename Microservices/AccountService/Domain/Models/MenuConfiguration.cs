namespace Ecosystem.AccountService.Domain.Models;

public partial class MenuConfiguration
{
    public long Id { get; set; }
    public string? MenuName { get; set; }
    public string? PageName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<Privilege> Privileges { get; } = new List<Privilege>();
}
