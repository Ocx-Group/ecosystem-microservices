using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Adapters;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
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
    private readonly IConfigurationServiceAdapter _configurationServiceAdapter;

    public GetAllTradingAcademyHandler(
        IProductRepository repo,
        IMapper mapper,
        ITenantContext tenant,
        IConfigurationServiceAdapter configurationServiceAdapter,
        ILogger<GetAllTradingAcademyHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _tenant = tenant;
        _configurationServiceAdapter = configurationServiceAdapter;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllTradingAcademyQuery request, CancellationToken ct)
    {
        var brandId = _tenant.TenantId;
        var configuration = await _configurationServiceAdapter
            .GetBrandConfigurationAsync(brandId, ct)
            ?? throw new InvalidOperationException(
                $"Active brand configuration not found for brand {brandId}.");

        var paymentGroup = configuration.TradingAcademyPaymentGroupId
            ?? throw new InvalidOperationException(
                $"Trading academy payment group is not configured for brand {brandId}.");

        var products = await _repo.GetAllWithPaymentGroup(paymentGroup, brandId);
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
