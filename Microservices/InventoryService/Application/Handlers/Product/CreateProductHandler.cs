using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.Product;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using Ecosystem.InventoryService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(
        IProductRepository productRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<CreateProductHandler> logger)
    {
        _productRepository = productRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(
        CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _mapper.Map<Domain.Models.Product>(request);
        product.BrandId = _tenantContext.TenantId;
        var created = await _productRepository.CreateProductAsync(product);
        return _mapper.Map<ProductDto>(created);
    }
}
