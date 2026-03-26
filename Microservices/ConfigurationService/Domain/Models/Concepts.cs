namespace Ecosystem.ConfigurationService.Domain.Models;

public partial class Concepts
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public bool? Active { get; set; }
    public bool Compression { get; set; }
    public long PaymentGroupId { get; set; }
    public int PayConcept { get; set; }
    public int CalculateBy { get; set; }
    public int BinaryType { get; set; }
    public decimal BinaryPercentage { get; set; }
    public int BinaryTopInfinity { get; set; }
    public decimal BinaryTop { get; set; }
    public string? BinaryProductsGroup { get; set; }
    public string? BinaryGrades { get; set; }
    public bool Equalization { get; set; }
    public DateTime DateConcept { get; set; }
    public bool IgnoreActivationOrder { get; set; }
    public bool? Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public long? BrandId { get; set; }
    public virtual ICollection<ConceptConfigurations> ConceptConfigurations { get; } = new List<ConceptConfigurations>();
    public virtual ICollection<PaidConcept> PaidConcepts { get; } = new List<PaidConcept>();
    public virtual PaymentGroups PaymentGroups { get; set; } = null!;
}
