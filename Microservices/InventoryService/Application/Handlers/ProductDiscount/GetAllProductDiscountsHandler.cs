using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductDiscount;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductDiscount;

public class GetAllProductDiscountsHandler
    : IRequestHandler<GetAllProductDiscountsQuery, ICollection<ProductDiscountDto>>
{
    private readonly IProductDiscountRepository _productDiscountRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductDiscountsHandler> _logger;

    public GetAllProductDiscountsHandler(
        IProductDiscountRepository productDiscountRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllProductDiscountsHandler> logger)
    {
        _productDiscountRepository = productDiscountRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductDiscountDto>> Handle(
        GetAllProductDiscountsQuery request, CancellationToken cancellationToken)
    {
        var discounts = await _productDiscountRepository.GetAllProductsDiscounts(_tenantContext.TenantId);
        return _mapper.Map<ICollection<ProductDiscountDto>>(discounts);
    }
}
