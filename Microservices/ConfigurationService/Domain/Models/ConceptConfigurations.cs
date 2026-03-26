namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class ConceptConfigurations
{
    public long Id { get; set; }
    public long ConceptId { get; set; }
    public int Level { get; set; }
    public decimal Percentage { get; set; }
    public int Equalization { get; set; }
    public bool Status { get; set; }
    public bool Compression { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? BrandId { get; set; }
    public virtual Concepts Concepts { get; set; } = null!;
}
