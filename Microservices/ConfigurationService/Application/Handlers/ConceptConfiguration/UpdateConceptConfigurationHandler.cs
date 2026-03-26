using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.ConceptConfiguration;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.ConceptConfiguration;

public class UpdateConceptConfigurationHandler : IRequestHandler<UpdateConceptConfigurationCommand, ConceptConfigurationDto?>
{
    private readonly IConceptConfigurationRepository _repository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateConceptConfigurationHandler> _logger;

    public UpdateConceptConfigurationHandler(
        IConceptConfigurationRepository repository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateConceptConfigurationHandler> logger)
    {
        _repository = repository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConceptConfigurationDto?> Handle(UpdateConceptConfigurationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetConceptConfigurationById(request.Id);
        if (entity is null) return null;

        entity.ConceptId = request.ConceptId;
        entity.Level = request.Level;
        entity.Percentage = request.Percentage;
        entity.Equalization = request.Equalization;
        entity.Status = request.Status;
        entity.Compression = request.Compression;
        entity.BrandId = _tenantContext.TenantId;

        var updated = await _repository.UpdateConceptConfiguration(entity);
        return _mapper.Map<ConceptConfigurationDto>(updated);
    }
}
