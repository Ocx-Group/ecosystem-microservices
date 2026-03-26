using AutoMapper;
using Ecosystem.InventoryService.Application.Commands.ProductAttributeValue;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductAttributeValue;

public class DeleteProductAttributeValueHandler
    : IRequestHandler<DeleteProductAttributeValueCommand, ProductAttributeValueDto?>
{
    private readonly IProductAttributeValueRepository _productAttributeValueRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductAttributeValueHandler> _logger;

    public DeleteProductAttributeValueHandler(
        IProductAttributeValueRepository productAttributeValueRepository,
        IMapper mapper,
        ILogger<DeleteProductAttributeValueHandler> logger)
    {
        _productAttributeValueRepository = productAttributeValueRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductAttributeValueDto?> Handle(
        DeleteProductAttributeValueCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productAttributeValueRepository.GetProductsAttributesValueById(request.Id);
        if (entity is null) return null;

        var deleted = await _productAttributeValueRepository.DeleteProductAttributeValueAsync(entity);
        return _mapper.Map<ProductAttributeValueDto>(deleted);
    }
}
