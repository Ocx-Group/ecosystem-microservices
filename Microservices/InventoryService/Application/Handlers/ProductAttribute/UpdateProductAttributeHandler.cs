using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductAttribute;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductAttribute;

public class UpdateProductAttributeHandler : IRequestHandler<UpdateProductAttributeCommand, ProductAttributeDto?>
{
    private readonly IProductAttributeRepository _productAttributeRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductAttributeHandler> _logger;

    public UpdateProductAttributeHandler(
        IProductAttributeRepository productAttributeRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateProductAttributeHandler> logger)
    {
        _productAttributeRepository = productAttributeRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductAttributeDto?> Handle(
        UpdateProductAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productAttributeRepository.GetProductsAttributesById(request.Id);
        if (entity is null) return null;

        entity.Attribute = request.Attribute;
        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.Type = request.Type;
        entity.Position = request.Position;
        entity.Color = request.Color;
        entity.State = request.State;
        entity.BrandId = _tenantContext.TenantId;

        var updated = await _productAttributeRepository.UpdateProductAttributeAsync(entity);
        return _mapper.Map<ProductAttributeDto>(updated);
    }
}
