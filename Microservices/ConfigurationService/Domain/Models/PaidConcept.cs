namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class PaidConcept
{
    public long Id { get; set; }
    public long ConceptId { get; set; }
    public DateTime Date { get; set; }
    public decimal? Total { get; set; }
    public long? BrandId { get; set; }
    public virtual Concepts Concepts { get; set; } = null!;
}
