using AutoMapper;
using Ecosystem.InventoryService.Application.Commands.Product;
using Ecosystem.InventoryService.Application.Commands.ProductAttribute;
using Ecosystem.InventoryService.Application.Commands.ProductAttributeValue;
using Ecosystem.InventoryService.Application.Commands.ProductBanner;
using Ecosystem.InventoryService.Application.Commands.ProductCategory;
using Ecosystem.InventoryService.Application.Commands.ProductCombination;
using Ecosystem.InventoryService.Application.Commands.ProductDiscount;
using Ecosystem.InventoryService.Application.Commands.ProductInventory;
using Ecosystem.InventoryService.Application.DTOs;
using Ecosystem.InventoryService.Domain.Models;
using Ecosystem.Domain.Core.Storage;

namespace Ecosystem.InventoryService.Application.Mappings;

public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        // Model -> DTO
        CreateMap<Domain.Models.Product, ProductDto>()
            .ForMember(d => d.ProductsCategory, opt => opt.MapFrom(s => s.Category))
            .ForMember(d => d.KeyWord, opt => opt.MapFrom(s => s.Keyword))
            .ForMember(d => d.Image, opt => opt.MapFrom(s => ObjectStorageUrl.Normalize(s.Image)));

        CreateMap<ProductsCategory, ProductCategoryDto>()
            .ForMember(d => d.IdCategory, opt => opt.MapFrom(s => s.Category));

        CreateMap<ProductsInventory, ProductInventoryDto>();

        CreateMap<ProductsAttribute, ProductAttributeDto>()
            .ForMember(d => d.IdAttributes, opt => opt.MapFrom(s => s.Attribute));

        CreateMap<ProductsAttributesValue, ProductAttributeValueDto>();
        CreateMap<ProductsDiscount, ProductDiscountDto>();
        CreateMap<ProductsBanner, ProductBannerDto>();
        CreateMap<ProductsCombination, ProductCombinationDto>();

        // Command -> Model
        CreateMap<CreateProductCommand, Domain.Models.Product>();
        CreateMap<CreateProductCategoryCommand, ProductsCategory>();
        CreateMap<CreateProductInventoryCommand, ProductsInventory>();
        CreateMap<CreateProductAttributeCommand, ProductsAttribute>();

        CreateMap<CreateProductAttributeValueCommand, ProductsAttributesValue>();
        CreateMap<CreateProductDiscountCommand, ProductsDiscount>();
        CreateMap<CreateProductBannerCommand, ProductsBanner>();
        CreateMap<CreateProductCombinationCommand, ProductsCombination>();
    }
}
