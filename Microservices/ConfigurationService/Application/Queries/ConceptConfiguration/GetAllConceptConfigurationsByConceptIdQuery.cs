using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.ConceptConfiguration;

public record GetAllConceptConfigurationsByConceptIdQuery(int ConceptId) : IRequest<ICollection<ConceptConfigurationDto>>;
