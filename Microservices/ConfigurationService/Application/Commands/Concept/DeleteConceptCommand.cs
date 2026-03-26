using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.Concept;

public record DeleteConceptCommand(int Id) : IRequest<ConceptDto?>;
