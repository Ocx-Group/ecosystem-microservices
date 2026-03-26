using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllSavingsPlansOneBHandler : IRequestHandler<GetAllSavingsPlansOneBQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllSavingsPlansOneBHandler> _logger;

    public GetAllSavingsPlansOneBHandler(IProductRepository repo, IMapper mapper, ILogger<GetAllSavingsPlansOneBHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllSavingsPlansOneBQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllSavingsPlansOneB();
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
