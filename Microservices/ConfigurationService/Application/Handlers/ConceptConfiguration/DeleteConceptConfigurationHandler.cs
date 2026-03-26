using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.ConceptConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.ConceptConfiguration;

public class DeleteConceptConfigurationHandler : IRequestHandler<DeleteConceptConfigurationCommand, ConceptConfigurationDto?>
{
    private readonly IConceptConfigurationRepository _repository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteConceptConfigurationHandler> _logger;

    public DeleteConceptConfigurationHandler(
        IConceptConfigurationRepository repository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<DeleteConceptConfigurationHandler> logger)
    {
        _repository = repository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConceptConfigurationDto?> Handle(DeleteConceptConfigurationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetConceptConfigurationById(request.Id);
        if (entity is null) return null;

        var deleted = await _repository.DeleteConceptConfiguration(entity);
        return _mapper.Map<ConceptConfigurationDto>(deleted);
    }
}
