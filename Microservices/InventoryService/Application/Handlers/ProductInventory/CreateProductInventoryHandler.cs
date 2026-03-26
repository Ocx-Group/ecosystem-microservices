using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductInventory;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductInventory;

public class CreateProductInventoryHandler : IRequestHandler<CreateProductInventoryCommand, ProductInventoryDto?>
{
    private readonly IProductInventoryRepository _productInventoryRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductInventoryHandler> _logger;

    public CreateProductInventoryHandler(
        IProductInventoryRepository productInventoryRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductInventoryHandler> logger)
    {
        _productInventoryRepository = productInventoryRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductInventoryDto?> Handle(
        CreateProductInventoryCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductsInventory>(request);
        entity.BrandId = _tenantContext.TenantId;
        var created = await _productInventoryRepository.CreateProductsInventoryAsync(entity);
        return _mapper.Map<ProductInventoryDto>(created);
    }
}
