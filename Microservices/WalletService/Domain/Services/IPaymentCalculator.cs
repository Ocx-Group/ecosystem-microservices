using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;
using Ecosystem.WalletService.Domain.ValueObjects;

namespace Ecosystem.WalletService.Domain.Services;

public interface IPaymentCalculator
{
    PaymentCalculation Calculate(
        List<ProductWalletDto> products,
        ICollection<ProductsRequests> requestedProducts);
}
