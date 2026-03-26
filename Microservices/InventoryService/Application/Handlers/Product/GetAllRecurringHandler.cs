using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.Product;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class GetAllRecurringHandler : IRequestHandler<GetAllRecurringQuery, ICollection<ProductDto>>
{
    private readonly IProductRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllRecurringHandler> _logger;

    public GetAllRecurringHandler(IProductRepository repo, IMapper mapper, ILogger<GetAllRecurringHandler> logger)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductDto>> Handle(GetAllRecurringQuery request, CancellationToken ct)
    {
        var products = await _repo.GetAllRecurring();
        return _mapper.Map<ICollection<ProductDto>>(products.OrderBy(p => p.SalePrice).ToList());
    }
}
