using AutoMapper;
using Ecosystem.InventoryService.Application.Commands.ProductCategory;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductCategory;

public class DeleteProductCategoryHandler : IRequestHandler<DeleteProductCategoryCommand, ProductCategoryDto?>
{
    private readonly IProductCategoryRepository _productCategoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductCategoryHandler> _logger;

    public DeleteProductCategoryHandler(
        IProductCategoryRepository productCategoryRepository,
        IMapper mapper,
        ILogger<DeleteProductCategoryHandler> logger)
    {
        _productCategoryRepository = productCategoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductCategoryDto?> Handle(
        DeleteProductCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productCategoryRepository.GetProductsCategoriesById(request.Id);
        if (entity is null) return null;

        var deleted = await _productCategoryRepository.DeleteProductCategoryAsync(entity);
        return _mapper.Map<ProductCategoryDto>(deleted);
    }
}
