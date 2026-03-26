using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.ProductDiscount;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductDiscount;

public class UpdateProductDiscountHandler : IRequestHandler<UpdateProductDiscountCommand, ProductDiscountDto?>
{
    private readonly IProductDiscountRepository _productDiscountRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductDiscountHandler> _logger;

    public UpdateProductDiscountHandler(
        IProductDiscountRepository productDiscountRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateProductDiscountHandler> logger)
    {
        _productDiscountRepository = productDiscountRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDiscountDto?> Handle(
        UpdateProductDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productDiscountRepository.GetProductsDiscountById(request.Id);
        if (entity is null) return null;

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.IdProduct = request.IdProduct;
        entity.Qualification = request.Qualification;
        entity.Percentage = request.Percentage;
        entity.PointsQualify = request.PointsQualify;
        entity.BinaryPoints = request.BinaryPoints;
        entity.Commissionable = request.Commissionable;
        entity.PCommissionable = request.PCommissionable;
        entity.PBinaryPoints = request.PBinaryPoints;
        entity.PPointsQualify = request.PPointsQualify;
        entity.BrandId = _tenantContext.TenantId;

        var updated = await _productDiscountRepository.UpdateProductDiscountAsync(entity);
        return _mapper.Map<ProductDiscountDto>(updated);
    }
}
