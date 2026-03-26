using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.Concept;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Concept;

public class GetAllConceptsHandler : IRequestHandler<GetAllConceptsQuery, ICollection<ConceptDto>>
{
    private readonly IConceptRepository _conceptRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllConceptsHandler> _logger;

    public GetAllConceptsHandler(
        IConceptRepository conceptRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllConceptsHandler> logger)
    {
        _conceptRepository = conceptRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ConceptDto>> Handle(GetAllConceptsQuery request, CancellationToken cancellationToken)
    {
        var concepts = await _conceptRepository.GetAllConcepts(_tenantContext.TenantId);
        return _mapper.Map<ICollection<ConceptDto>>(concepts);
    }
}
