using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllServicesHandler : IRequestHandler<GetAllServicesQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllServicesHandler> _logger;

    public GetAllServicesHandler(IProductRepository repo, IMapper mapper, ILogger<GetAllServicesHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllServicesQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllServices();
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
