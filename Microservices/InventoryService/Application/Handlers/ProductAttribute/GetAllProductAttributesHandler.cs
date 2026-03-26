using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductAttribute;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductAttribute;

public class GetAllProductAttributesHandler
    : IRequestHandler<GetAllProductAttributesQuery, ICollection<ProductAttributeDto>>
{
    private readonly IProductAttributeRepository _productAttributeRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductAttributesHandler> _logger;

    public GetAllProductAttributesHandler(
        IProductAttributeRepository productAttributeRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetAllProductAttributesHandler> logger)
    {
        _productAttributeRepository = productAttributeRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductAttributeDto>> Handle(
        GetAllProductAttributesQuery request, CancellationToken cancellationToken)
    {
        var attributes = await _productAttributeRepository.GetAllProductsAttributes(_tenantContext.TenantId);
        return _mapper.Map<ICollection<ProductAttributeDto>>(attributes);
    }
}
