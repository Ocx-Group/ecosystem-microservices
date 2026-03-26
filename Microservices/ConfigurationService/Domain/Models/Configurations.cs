namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class Configurations
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string Value { get; set; } = null!;
    public bool Status { get; set; }
    public string DefaultValue { get; set; } = null!;
    public long? BrandId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
