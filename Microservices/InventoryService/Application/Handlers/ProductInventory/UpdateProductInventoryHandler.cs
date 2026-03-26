using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductInventory;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductInventory;

public class UpdateProductInventoryHandler : IRequestHandler<UpdateProductInventoryCommand, ProductInventoryDto?>
{
    private readonly IProductInventoryRepository _productInventoryRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductInventoryHandler> _logger;

    public UpdateProductInventoryHandler(
        IProductInventoryRepository productInventoryRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateProductInventoryHandler> logger)
    {
        _productInventoryRepository = productInventoryRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductInventoryDto?> Handle(
        UpdateProductInventoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productInventoryRepository.GetProductsInventoryById(request.Id);
        if (entity is null) return null;

        entity.IdProduct = request.IdProduct;
        entity.Ingress = request.Ingress;
        entity.Egress = request.Egress;
        entity.Support = request.Support;
        entity.Note = request.Note;
        entity.Type = request.Type;
        entity.Date = request.Date;
        entity.IdCombination = request.IdCombination;
        entity.BrandId = _tenantContext.TenantId;

        var updated = await _productInventoryRepository.UpdateProductInventoryAsync(entity);
        return _mapper.Map<ProductInventoryDto>(updated);
    }
}
