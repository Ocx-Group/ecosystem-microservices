using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Concept;

public record CreateConceptCommand : IRequest<ConceptDto>
{
    public string Name { get; init; } = null!;
    public int PaymentGroupId { get; init; }
    public int PayConcept { get; init; }
    public int CalculateBy { get; init; }
    public bool Compression { get; init; }
    public bool Equalization { get; init; }
    public bool IgnoreActivationOrder { get; init; }
    public bool Active { get; init; }
}
