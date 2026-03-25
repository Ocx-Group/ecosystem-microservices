namespace Ecosystem.AccountService.Domain.Models;

public partial class Privilege
{
    public long Id { get; set; }
    public long RolId { get; set; }
    public long MenuConfigurationId { get; set; }
    public bool CanCreate { get; set; }
    public bool CanEdit { get; set; }
    public bool CanRead { get; set; }
    public bool CanDelete { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual MenuConfiguration MenuConfiguration { get; set; } = null!;
    public virtual Role Rol { get; set; } = null!;
}
