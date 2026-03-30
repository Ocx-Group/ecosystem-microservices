using Ecosystem.WalletService.Domain.DTOs.ProductWalletDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequest;

namespace Ecosystem.WalletService.Domain.Services;

public interface IInvoiceDetailFactory
{
    List<InvoiceDetailsTransactionRequest> CreateDetails(
        List<ProductWalletDto> products,
        ICollection<ProductsRequests> requestedProducts,
        long brandId);

    InvoiceDetailsTransactionRequest CreateDetail(
        ProductWalletDto product,
        ProductsRequests requestedProduct,
        long brandId);
}
