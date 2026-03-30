using Ecosystem.WalletService.Domain.Models;
using Ecosystem.WalletService.Domain.Responses;

namespace Ecosystem.WalletService.Application.Adapters;

public interface IPdfService
{
    /// <summary>
    /// Generates a PDF from an HTML template stored in DB, hydrated with brand config + transaction data.
    /// </summary>
    Task<byte[]> GenerateFromTemplate(string templateKey, long brandId, object data);

    /// <summary>
    /// Regenerates an invoice PDF from stored invoice data (legacy support).
    /// </summary>
    Task<byte[]> RegenerateInvoice(UserInfoResponse user, Invoice invoice);
}
