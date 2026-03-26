using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductDiscount;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductDiscount;

public class GetAllProductDiscountsByProductIdHandler
    : IRequestHandler<GetAllProductDiscountsByProductIdQuery, ICollection<ProductDiscountDto>>
{
    private readonly IProductDiscountRepository _productDiscountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllProductDiscountsByProductIdHandler> _logger;

    public GetAllProductDiscountsByProductIdHandler(
        IProductDiscountRepository productDiscountRepository,
        IMapper mapper,
        ILogger<GetAllProductDiscountsByProductIdHandler> logger)
    {
        _productDiscountRepository = productDiscountRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductDiscountDto>> Handle(
        GetAllProductDiscountsByProductIdQuery request, CancellationToken cancellationToken)
    {
        var discounts = await _productDiscountRepository.GetAllProductsDiscountsByProductId(request.ProductId);
        return _mapper.Map<ICollection<ProductDiscountDto>>(discounts);
    }
}
