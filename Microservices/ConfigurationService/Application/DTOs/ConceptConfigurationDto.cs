namespace Ecosystem.ConfigurationService.Application.DTOs;

public class ConceptConfigurationDto
{
    public int Id { get; set; }
    public int ConceptId { get; set; }
    public int Level { get; set; }
    public decimal Percentage { get; set; }
    public int Equalization { get; set; }
    public bool Status { get; set; }
    public bool Compression { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public virtual ConceptDto Concept { get; set; } = null!;
}
