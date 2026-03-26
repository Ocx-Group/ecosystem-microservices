using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllWithPaymentGroupHandler : IRequestHandler<GetAllWithPaymentGroupQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenant;
    private readonly ILogger<GetAllWithPaymentGroupHandler> _logger;

    public GetAllWithPaymentGroupHandler(IProductRepository repo, IMapper mapper, ITenantContext tenant, ILogger<GetAllWithPaymentGroupHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _tenant = tenant;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllWithPaymentGroupQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllWithPaymentGroup(request.PaymentGroupId, _tenant.TenantId);
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
