namespace Ecosystem.ConfigurationService.Domain.Models;

public class Brand
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
    public bool? IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
