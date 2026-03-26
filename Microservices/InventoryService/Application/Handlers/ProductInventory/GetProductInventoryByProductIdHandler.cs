using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductInventory;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductInventory;

public class GetProductInventoryByProductIdHandler
    : IRequestHandler<GetProductInventoryByProductIdQuery, ICollection<ProductInventoryDto>>
{
    private readonly IProductInventoryRepository _productInventoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductInventoryByProductIdHandler> _logger;

    public GetProductInventoryByProductIdHandler(
        IProductInventoryRepository productInventoryRepository,
        IMapper mapper,
        ILogger<GetProductInventoryByProductIdHandler> logger)
    {
        _productInventoryRepository = productInventoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductInventoryDto>> Handle(
        GetProductInventoryByProductIdQuery request, CancellationToken cancellationToken)
    {
        var inventory = await _productInventoryRepository.GetProductsInventoryByProductId(request.ProductId);
        return _mapper.Map<ICollection<ProductInventoryDto>>(inventory);
    }
}
