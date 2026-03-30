using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Services;

namespace Ecosystem.WalletService.Application.Services;

public class InvoiceDetailFactory : IInvoiceDetailFactory
{
    public List<InvoiceDetailsTransactionRequest> CreateDetails(
        List<ProductWalletDto> products,
        ICollection<ProductsRequests> requestedProducts,
        long brandId)
    {
        var details = new List<InvoiceDetailsTransactionRequest>();

        foreach (var product in products)
        {
            var requested = requestedProducts.FirstOrDefault(x => x.IdProduct == product.Id);
            if (requested is null) continue;

            details.Add(CreateDetail(product, requested, brandId));
        }

        return details;
    }

    public InvoiceDetailsTransactionRequest CreateDetail(
        ProductWalletDto product,
        ProductsRequests requestedProduct,
        long brandId)
    {
        return new InvoiceDetailsTransactionRequest
        {
            ProductId = product.Id,
            PaymentGroupId = product.PaymentGroup,
            AccumMinPurchase = product.AcumCompMin,
            ProductName = product.Name ?? string.Empty,
            ProductPrice = product.SalePrice,
            ProductPriceBtc = Constants.EmptyValue,
            ProductIva = product.Tax,
            ProductQuantity = requestedProduct.Count,
            ProductCommissionable = product.CommissionableValue,
            BinaryPoints = product.BinaryPoints,
            ProductPoints = product.ValuePoints,
            ProductDiscount = product.ProductDiscount,
            CombinationId = Constants.EmptyValue,
            ProductPack = product.ProductPacks,
            BaseAmount = product.BaseAmount * requestedProduct.Count,
            DailyPercentage = product.DailyPercentage,
            WaitingDays = product.DaysWait,
            DaysToPayQuantity = Constants.DaysToPayQuantity,
            ProductStart = false,
            BrandId = brandId
        };
    }
}
