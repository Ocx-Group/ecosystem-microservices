using AutoMapper;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Application.Queries.ProductAttributeValue;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.ProductAttributeValue;

public class GetProductAttributeValuesByAttributeIdHandler
    : IRequestHandler<GetProductAttributeValuesByAttributeIdQuery, ICollection<ProductAttributeValueDto>>
{
    private readonly IProductAttributeValueRepository _productAttributeValueRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetProductAttributeValuesByAttributeIdHandler> _logger;

    public GetProductAttributeValuesByAttributeIdHandler(
        IProductAttributeValueRepository productAttributeValueRepository,
        IMapper mapper,
        ILogger<GetProductAttributeValuesByAttributeIdHandler> logger)
    {
        _productAttributeValueRepository = productAttributeValueRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<ProductAttributeValueDto>> Handle(
        GetProductAttributeValuesByAttributeIdQuery request, CancellationToken cancellationToken)
    {
        var values = await _productAttributeValueRepository.GetProductsAttributesValuesByAttributeId(request.AttributeId);
        return _mapper.Map<ICollection<ProductAttributeValueDto>>(values);
    }
}
