using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductAttribute;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductAttribute;

public class CreateProductAttributeHandler : IRequestHandler<CreateProductAttributeCommand, ProductAttributeDto?>
{
    private readonly IProductAttributeRepository _productAttributeRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductAttributeHandler> _logger;

    public CreateProductAttributeHandler(
        IProductAttributeRepository productAttributeRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductAttributeHandler> logger)
    {
        _productAttributeRepository = productAttributeRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductAttributeDto?> Handle(
        CreateProductAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductsAttribute>(request);
        entity.BrandId = _tenantContext.TenantId;
        var created = await _productAttributeRepository.CreateProductsAttributesAsync(entity);
        return _mapper.Map<ProductAttributeDto>(created);
    }
}
