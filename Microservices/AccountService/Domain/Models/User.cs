namespace Ecosystem.AccountService.Domain.Models;

public partial class User
{
    public long Id { get; set; }
    public long RolId { get; set; }
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? SecretQuestion { get; set; }
    public string? SecretAnswer { get; set; }
    public string? LastActivity { get; set; }
    public string? Phone { get; set; }
    public string? Address { get; set; }
    public string? Observation { get; set; }
    public bool? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? ImageProfileUrl { get; set; }
    public long BrandId { get; set; }
    public virtual Brand Brand { get; set; } = null!;
    public virtual Role Rol { get; set; } = null!;
}
