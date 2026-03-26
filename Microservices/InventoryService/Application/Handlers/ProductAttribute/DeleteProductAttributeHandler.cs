using AutoMapper;
using Ecosystem.InventoryService.Application.Commands.ProductAttribute;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductAttribute;

public class DeleteProductAttributeHandler : IRequestHandler<DeleteProductAttributeCommand, ProductAttributeDto?>
{
    private readonly IProductAttributeRepository _productAttributeRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductAttributeHandler> _logger;

    public DeleteProductAttributeHandler(
        IProductAttributeRepository productAttributeRepository,
        IMapper mapper,
        ILogger<DeleteProductAttributeHandler> logger)
    {
        _productAttributeRepository = productAttributeRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductAttributeDto?> Handle(
        DeleteProductAttributeCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productAttributeRepository.GetProductsAttributesById(request.Id);
        if (entity is null) return null;

        var deleted = await _productAttributeRepository.DeleteProductAttributeAsync(entity);
        return _mapper.Map<ProductAttributeDto>(deleted);
    }
}
