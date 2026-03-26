using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllProductsByIdsHandler : IRequestHandler<GetAllProductsByIdsQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductsByIdsHandler> _logger;

    public GetAllProductsByIdsHandler(IProductRepository repo, IMapper mapper, ILogger<GetAllProductsByIdsHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllProductsByIdsQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllProductsByIds(request.ProductIds);
        return _mapper.Map<ICollection<ProductDto>>(products);
    }
}
