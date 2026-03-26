using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllMembershipHandler : IRequestHandler<GetAllMembershipQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenant;
    private readonly ILogger<GetAllMembershipHandler> _logger;

    public GetAllMembershipHandler(IProductRepository repo, IMapper mapper, ITenantContext tenant, ILogger<GetAllMembershipHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _tenant = tenant;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllMembershipQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAlMembership(_tenant.TenantId);
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
