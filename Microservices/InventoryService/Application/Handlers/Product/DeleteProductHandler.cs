using AutoMapper;
using Ecosystem.InventoryService.Application.Commands.Product;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductHandler> _logger;

    public DeleteProductHandler(
        IProductRepository productRepository,
        IMapper mapper,
        ILogger<DeleteProductHandler> logger)
    {
        _productRepository = productRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(
        DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductsById(request.Id);
        if (product is null) return null;

        var deleted = await _productRepository.DeleteProduct(product);
        return _mapper.Map<ProductDto>(deleted);
    }
}
