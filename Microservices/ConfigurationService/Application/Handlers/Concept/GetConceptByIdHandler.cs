using AutoMapper;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Application.Queries.Concept;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Concept;

public class GetConceptByIdHandler : IRequestHandler<GetConceptByIdQuery, ConceptDto?>
{
    private readonly IConceptRepository _conceptRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetConceptByIdHandler> _logger;

    public GetConceptByIdHandler(
        IConceptRepository conceptRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetConceptByIdHandler> logger)
    {
        _conceptRepository = conceptRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConceptDto?> Handle(GetConceptByIdQuery request, CancellationToken cancellationToken)
    {
        var concept = await _conceptRepository.GetConceptById(request.Id);
        if (concept is null) return null;

        return _mapper.Map<ConceptDto>(concept);
    }
}
