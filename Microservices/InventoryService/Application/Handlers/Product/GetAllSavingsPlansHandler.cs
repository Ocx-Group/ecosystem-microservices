using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllSavingsPlansHandler : IRequestHandler<GetAllSavingsPlansQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSavingsPlansHandler> _logger;

    public GetAllSavingsPlansHandler(IProductRepository repo, IMapper mapper, ILogger<GetAllSavingsPlansHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllSavingsPlansQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllSavingsPlans();
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
