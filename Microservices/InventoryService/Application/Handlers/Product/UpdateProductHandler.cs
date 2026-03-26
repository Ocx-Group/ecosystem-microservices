using AutoMapper;
using Ecosystem.Domain.Core.MultiTenancy;
using Ecosystem.InventoryService.Application.Commands.Product;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.InventoryService.Application.Handlers.Product;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, ProductDto?>
{
    private readonly IProductRepository _productRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateProductHandler> _logger;

    public UpdateProductHandler(
        IProductRepository productRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<UpdateProductHandler> logger)
    {
        _productRepository = productRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto?> Handle(
        UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductsById(request.Id);
        if (product is null) return null;

        product.CategoryId = request.CategoryId;
        product.SalePrice = request.SalePrice;
        product.CommissionableValue = request.CommissionableValue;
        product.BinaryPoints = request.BinaryPoints;
        product.ValuePoints = request.ValuePoints;
        product.Tax = request.Tax;
        product.Inventory = request.Inventory;
        product.PaymentGroup = request.PaymentGroup;
        product.AcumCompMin = request.AcumCompMin;
        product.Weight = request.Weight;
        product.Offer = request.Offer;
        product.Comment = request.Comment;
        product.HideCommissionable = request.HideCommissionable;
        product.HidePoint = request.HidePoint;
        product.ActiveHtmlContent = request.ActiveHtmlContent;
        product.ActiveZoomPhotos = request.ActiveZoomPhotos;
        product.Visible = request.Visible;
        product.VisiblePublic = request.VisiblePublic;
        product.ProductType = request.ProductType;
        product.State = request.State;
        product.ActivateCombinations = request.ActivateCombinations;
        product.ProductPacks = request.ProductPacks;
        product.BaseAmount = request.BaseAmount;
        product.DailyPercentage = request.DailyPercentage;
        product.DaysWait = request.DaysWait;
        product.AmountDayPay = request.AmountDayPay;
        product.RecurringProduct = request.RecurringProduct;
        product.ProductHome = request.ProductHome;
        product.AssociatedQualification = request.AssociatedQualification;
        product.Name = request.Name;
        product.Description = request.Description;
        product.DescriptionHtml = request.DescriptionHtml;
        product.ProductCode = request.ProductCode;
        product.Keyword = request.Keyword;
        product.Image = request.Image;
        product.ModelTwoPercentage = request.ModelTwoPercentage;
        product.BrandId = _tenantContext.TenantId;

        var updated = await _productRepository.UpdateProductAsync(product);
        return _mapper.Map<ProductDto>(updated);
    }
}
