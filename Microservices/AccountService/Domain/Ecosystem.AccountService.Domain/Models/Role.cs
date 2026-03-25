namespace Ecosystem.AccountService.Domain.Models;

public partial class Role
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<Privilege> Privileges { get; } = new List<Privilege>();
    public virtual ICollection<User> Users { get; } = new List<User>();
}
