using AutoMapper;
using Ecosystem.InventoryService.Application.Commands.ProductCombination;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductCombination;

public class DeleteProductCombinationHandler : IRequestHandler<DeleteProductCombinationCommand, ProductCombinationDto?>
{
    private readonly IProductCombinationRepository _productCombinationRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteProductCombinationHandler> _logger;

    public DeleteProductCombinationHandler(
        IProductCombinationRepository productCombinationRepository,
        IMapper mapper,
        ILogger<DeleteProductCombinationHandler> logger)
    {
        _productCombinationRepository = productCombinationRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductCombinationDto?> Handle(
        DeleteProductCombinationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _productCombinationRepository.GetProductsCombinationsById(request.Id);
        if (entity is null) return null;

        var deleted = await _productCombinationRepository.DeleteProductCombinationAsync(entity);
        return _mapper.Map<ProductCombinationDto>(deleted);
    }
}
