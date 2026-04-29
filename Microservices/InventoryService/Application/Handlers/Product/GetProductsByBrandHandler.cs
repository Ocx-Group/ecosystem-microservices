using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetProductsByBrandHandler : IRequestHandler<GetProductsByBrandQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenant;

    public GetProductsByBrandHandler(
        IProductRepository repo,
        IMapper mapper,
        ITenantContext tenant,
        ILogger<GetProductsByBrandHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _tenant = tenant;
    }

    public async Task<ICollection<ProductDto>> Handle(GetProductsByBrandQuery request, CancellationToken cancellationToken)
    {
        var products = await _repo.GetProductsByBrand(
            _tenant.TenantId,
            request.ProductIds,
            request.PaymentGroupIds,
            request.ProductType,
            request.State,
            request.Visible,
            request.VisiblePublic,
            request.IncludeDeleted);

        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
