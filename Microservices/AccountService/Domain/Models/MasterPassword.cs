namespace Ecosystem.AccountService.Domain.Models;

public class MasterPassword
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public string Password { get; set; }
    public string Algorithm { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
