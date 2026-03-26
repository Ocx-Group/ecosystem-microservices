using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductCombination;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductCombination;

public class CreateProductCombinationHandler : IRequestHandler<CreateProductCombinationCommand, ProductCombinationDto?>
{
    private readonly IProductCombinationRepository _productCombinationRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductCombinationHandler> _logger;

    public CreateProductCombinationHandler(
        IProductCombinationRepository productCombinationRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductCombinationHandler> logger)
    {
        _productCombinationRepository = productCombinationRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductCombinationDto?> Handle(
        CreateProductCombinationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductsCombination>(request);
        entity.BrandId = _tenantContext.TenantId;
        var created = await _productCombinationRepository.CreateProductsCombinationAsync(entity);
        return _mapper.Map<ProductCombinationDto>(created);
    }
}
