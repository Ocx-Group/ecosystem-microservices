using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductCombination;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductCombination;

public class GetProductCombinationsByProductIdHandler
    : IRequestHandler<GetProductCombinationsByProductIdQuery, ICollection<ProductCombinationDto>>
{
    private readonly IProductCombinationRepository _productCombinationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductCombinationsByProductIdHandler> _logger;

    public GetProductCombinationsByProductIdHandler(
        IProductCombinationRepository productCombinationRepository,
        IMapper mapper,
        ILogger<GetProductCombinationsByProductIdHandler> logger)
    {
        _productCombinationRepository = productCombinationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductCombinationDto>> Handle(
        GetProductCombinationsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var combinations = await _productCombinationRepository.GetProductsCombinationsByProductId(request.ProductId);
        return _mapper.Map<ICollection<ProductCombinationDto>>(combinations);
    }
}
