using AutoMapper;
using Ecosystem.InventoryService.Application.Commands.ProductDiscount;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductDiscount;

public class DeleteProductDiscountHandler : IRequestHandler<DeleteProductDiscountCommand, ProductDiscountDto?>
{
    private readonly IProductDiscountRepository _productDiscountRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductDiscountHandler> _logger;

    public DeleteProductDiscountHandler(
        IProductDiscountRepository productDiscountRepository,
        IMapper mapper,
        ILogger<DeleteProductDiscountHandler> logger)
    {
        _productDiscountRepository = productDiscountRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDiscountDto?> Handle(
        DeleteProductDiscountCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productDiscountRepository.GetProductsDiscountById(request.Id);
        if (entity is null) return null;

        var deleted = await _productDiscountRepository.DeleteProductDiscountAsync(entity);
        return _mapper.Map<ProductDiscountDto>(deleted);
    }
}
