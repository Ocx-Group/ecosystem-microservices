using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Constants;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllTradingAcademyHandler : IRequestHandler<GetAllTradingAcademyQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenant;
    private readonly ILogger<GetAllTradingAcademyHandler> _logger;

    public GetAllTradingAcademyHandler(IProductRepository repo, IMapper mapper, ITenantContext tenant, ILogger<GetAllTradingAcademyHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _tenant = tenant;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllTradingAcademyQuery request, CancellationToken ct)
    {
        var paymentGroup = _tenant.TenantId == 1
            ? InventoryConstants.TradingAcademyForEcosystem
            : InventoryConstants.TradingAcademyForRecycoin;

        var products = await _repo.GetAllTradingAcademy(paymentGroup);
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
