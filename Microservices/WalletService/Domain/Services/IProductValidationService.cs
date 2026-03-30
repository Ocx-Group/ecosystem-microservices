using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Domain.Services;

public record ProductValidationResult(
    bool IsSuccess,
    List<ProductWalletDto>? Products = null,
    string? ErrorMessage = null
)
{
    public static ProductValidationResult Success(List<ProductWalletDto> products) => new(true, products);
    public static ProductValidationResult Fail(string message) => new(false, ErrorMessage: message);
}

public interface IProductValidationService
{
    Task<ProductValidationResult> ValidateAndGetProducts(
        ICollection<ProductsRequests> requestedProducts,
        long brandId);
}
