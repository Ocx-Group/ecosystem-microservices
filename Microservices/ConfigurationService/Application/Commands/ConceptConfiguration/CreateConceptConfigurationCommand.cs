using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.ConceptConfiguration;

public record CreateConceptConfigurationCommand : IRequest<ConceptConfigurationDto>
{
    public int ConceptId { get; init; }
    public int Level { get; init; }
    public decimal Percentage { get; init; }
    public int Equalization { get; init; }
    public bool Status { get; init; }
    public bool Compression { get; init; }
}
