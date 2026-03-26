using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductAttributeValue;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductAttributeValue;

public class CreateProductAttributeValueHandler
    : IRequestHandler<CreateProductAttributeValueCommand, ProductAttributeValueDto?>
{
    private readonly IProductAttributeValueRepository _productAttributeValueRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductAttributeValueHandler> _logger;

    public CreateProductAttributeValueHandler(
        IProductAttributeValueRepository productAttributeValueRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductAttributeValueHandler> logger)
    {
        _productAttributeValueRepository = productAttributeValueRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductAttributeValueDto?> Handle(
        CreateProductAttributeValueCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductsAttributesValue>(request);
        entity.BrandId = _tenantContext.TenantId;
        var created = await _productAttributeValueRepository.CreateProductAttributeValueAsync(entity);
        return _mapper.Map<ProductAttributeValueDto>(created);
    }
}
