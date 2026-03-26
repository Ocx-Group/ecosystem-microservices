using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Concept;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.ConfigurationService.Domain.Models;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Concept;

public class CreateConceptHandler : IRequestHandler<CreateConceptCommand, ConceptDto>
{
    private readonly IConceptRepository _conceptRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateConceptHandler> _logger;

    public CreateConceptHandler(
        IConceptRepository conceptRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateConceptHandler> logger)
    {
        _conceptRepository = conceptRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConceptDto> Handle(CreateConceptCommand request, CancellationToken cancellationToken)
    {
        var concept = _mapper.Map<Concepts>(request);
        concept.DateConcept = DateTime.Now;
        concept.BrandId = _tenantContext.TenantId;

        var created = await _conceptRepository.CreateConcept(concept);
        return _mapper.Map<ConceptDto>(created);
    }
}
