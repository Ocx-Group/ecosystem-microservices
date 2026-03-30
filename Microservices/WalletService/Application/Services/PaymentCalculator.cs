using Ecosystem.WalletService.Domain.Constants;
using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.Services;
using Ecosystem.WalletService.Domain.ValueObjects;

namespace Ecosystem.WalletService.Application.Services;

public class PaymentCalculator : IPaymentCalculator
{
    private const int EcoPoolCategoryId = 2;

    public PaymentCalculation Calculate(
        List<ProductWalletDto> products,
        ICollection<ProductsRequests> requestedProducts)
    {
        decimal debit = 0;
        decimal points = 0;
        decimal commissionable = 0;
        decimal totalBaseAmount = 0;
        short origin = 0;

        foreach (var product in products)
        {
            var requested = requestedProducts.FirstOrDefault(x => x.IdProduct == product.Id);
            if (requested is null) continue;

            var tax = product.Tax;
            debit += (int)((product.SalePrice * requested.Count) * (1 + (tax / 100)));
            points += product.BinaryPoints * requested.Count;
            commissionable += product.CommissionableValue * requested.Count;
            totalBaseAmount += product.BaseAmount * requested.Count;

            if (product.CategoryId == EcoPoolCategoryId)
                origin = (short)Constants.OriginEcoPoolPurchase;
        }

        return new PaymentCalculation(debit, points, commissionable, origin, totalBaseAmount);
    }
}
