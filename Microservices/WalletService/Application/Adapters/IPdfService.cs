using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

// TODO: Migrate PDF generation services
public interface IPdfService
{
    Task<byte[]> RegenerateInvoice(UserInfoResponse user, Invoice invoice);
}
