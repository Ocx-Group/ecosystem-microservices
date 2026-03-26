using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.ConceptConfiguration;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.ConceptConfiguration;

public class GetAllConceptConfigurationsByConceptIdHandler : IRequestHandler<GetAllConceptConfigurationsByConceptIdQuery, ICollection<ConceptConfigurationDto>>
{
    private readonly IConceptConfigurationRepository _repository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllConceptConfigurationsByConceptIdHandler> _logger;

    public GetAllConceptConfigurationsByConceptIdHandler(
        IConceptConfigurationRepository repository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllConceptConfigurationsByConceptIdHandler> logger)
    {
        _repository = repository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ConceptConfigurationDto>> Handle(GetAllConceptConfigurationsByConceptIdQuery request, CancellationToken cancellationToken)
    {
        var configurations = await _repository.GetAllConceptConfigurationByConceptId(request.ConceptId, _tenantContext.TenantId);
        return _mapper.Map<ICollection<ConceptConfigurationDto>>(configurations);
    }
}
