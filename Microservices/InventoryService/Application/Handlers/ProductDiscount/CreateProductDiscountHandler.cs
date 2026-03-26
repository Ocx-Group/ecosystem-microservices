using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductDiscount;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductDiscount;

public class CreateProductDiscountHandler : IRequestHandler<CreateProductDiscountCommand, ProductDiscountDto?>
{
    private readonly IProductDiscountRepository _productDiscountRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductDiscountHandler> _logger;

    public CreateProductDiscountHandler(
        IProductDiscountRepository productDiscountRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductDiscountHandler> logger)
    {
        _productDiscountRepository = productDiscountRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDiscountDto?> Handle(
        CreateProductDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<ProductsDiscount>(request);
        entity.BrandId = _tenantContext.TenantId;
        var created = await _productDiscountRepository.CreateProductsDiscountAsync(entity);
        return _mapper.Map<ProductDiscountDto>(created);
    }
}
