using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllProductsAdminHandler : IRequestHandler<GetAllProductsAdminQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenant;
    private readonly ILogger<GetAllProductsAdminHandler> _logger;

    public GetAllProductsAdminHandler(IProductRepository repo, IMapper mapper, ITenantContext tenant, ILogger<GetAllProductsAdminHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _tenant = tenant;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllProductsAdminQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllProductsAdmin(_tenant.TenantId);
        return _mapper.Map<ICollection<ProductDto>>(products);
    }
}
