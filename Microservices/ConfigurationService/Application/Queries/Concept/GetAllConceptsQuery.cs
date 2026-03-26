using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.Concept;

public record GetAllConceptsQuery : IRequest<ICollection<ConceptDto>>;
