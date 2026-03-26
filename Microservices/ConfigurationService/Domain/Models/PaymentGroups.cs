namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class PaymentGroups
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    public bool Status { get; set; }
    public long? BrandId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ICollection<Concepts> Concepts { get; } = new List<Concepts>();
}
