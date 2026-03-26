using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Queries.Concept;

public record GetConceptByIdQuery(int Id) : IRequest<ConceptDto?>;
