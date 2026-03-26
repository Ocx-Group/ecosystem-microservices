using AutoMapper;
using Ecosystem.ConfigurationService.Application.Commands.Concept;
using Ecosystem.ConfigurationService.Application.DTOs;
using Ecosystem.ConfigurationService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.ConfigurationService.Application.Handlers.Concept;

public class UpdateConceptHandler : IRequestHandler<UpdateConceptCommand, ConceptDto?>
{
    private readonly IConceptRepository _conceptRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateConceptHandler> _logger;

    public UpdateConceptHandler(
        IConceptRepository conceptRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateConceptHandler> logger)
    {
        _conceptRepository = conceptRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ConceptDto?> Handle(UpdateConceptCommand request, CancellationToken cancellationToken)
    {
        var concept = await _conceptRepository.GetConceptById(request.Id);
        if (concept is null) return null;

        concept.Name = request.Name;
        concept.PaymentGroupId = request.PaymentGroupId;
        concept.PayConcept = request.PayConcept;
        concept.CalculateBy = request.CalculateBy;
        concept.Compression = request.Compression;
        concept.Equalization = request.Equalization;
        concept.IgnoreActivationOrder = request.IgnoreActivationOrder;
        concept.Active = request.Active;
        concept.BrandId = _tenantContext.TenantId;

        var updated = await _conceptRepository.UpdateConcept(concept);
        return _mapper.Map<ConceptDto>(updated);
    }
}
