namespace Ecosystem.AccountService.Domain.Models;

public partial class LoginMovement
{
    public long Id { get; set; }
    public long AffiliateId { get; set; }
    public string? BrowserInfo { get; set; }
    public string? OperatingSystem { get; set; }
    public string IpAddress { get; set; } = null!;
    public bool Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long BrandId { get; set; }
    public virtual Brand Brand { get; set; } = null!;
}
