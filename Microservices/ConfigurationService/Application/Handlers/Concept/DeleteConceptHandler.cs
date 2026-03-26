using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Concept;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Concept;

public class DeleteConceptHandler : IRequestHandler<DeleteConceptCommand, ConceptDto?>
{
    private readonly IConceptRepository _conceptRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteConceptHandler> _logger;

    public DeleteConceptHandler(
        IConceptRepository conceptRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<DeleteConceptHandler> logger)
    {
        _conceptRepository = conceptRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConceptDto?> Handle(DeleteConceptCommand request, CancellationToken cancellationToken)
    {
        var concept = await _conceptRepository.GetConceptById(request.Id);
        if (concept is null) return null;

        var deleted = await _conceptRepository.DeleteConcept(concept);
        return _mapper.Map<ConceptDto>(deleted);
    }
}
