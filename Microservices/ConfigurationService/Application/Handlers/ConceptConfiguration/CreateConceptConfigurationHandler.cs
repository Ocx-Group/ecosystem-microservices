using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.ConceptConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.ConceptConfiguration;

public class CreateConceptConfigurationHandler : IRequestHandler<CreateConceptConfigurationCommand, ConceptConfigurationDto>
{
    private readonly IConceptConfigurationRepository _repository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateConceptConfigurationHandler> _logger;

    public CreateConceptConfigurationHandler(
        IConceptConfigurationRepository repository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateConceptConfigurationHandler> logger)
    {
        _repository = repository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConceptConfigurationDto> Handle(CreateConceptConfigurationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ConceptConfigurations>(request);
        entity.BrandId = _tenantContext.TenantId;

        var created = await _repository.CreateConceptConfiguration(entity);
        return _mapper.Map<ConceptConfigurationDto>(created);
    }
}
