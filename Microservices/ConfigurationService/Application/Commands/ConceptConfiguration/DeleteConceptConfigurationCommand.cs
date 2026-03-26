using Ecosystem.ConfigurationService.Application.DTOs;
using MediatR;

namespace Ecosystem.ConfigurationService.Application.Commands.ConceptConfiguration;

public record DeleteConceptConfigurationCommand(int Id) : IRequest<ConceptConfigurationDto?>;
