using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IInvoiceDetailRepository
{
    Task<List<InvoicesDetail>> GetAllInvoiceDetailAsync();
    Task<InvoicesDetail> CreateInvoiceDetailAsync(InvoicesDetail                 invoice);
    Task<List<InvoicesDetail>> CreateBulkInvoiceDetailAsync(List<InvoicesDetail> requests);
    Task<List<InvoicesDetail>> GetInvoiceDetailByInvoiceIdAsync(long               invoiceId);

    Task<List<InvoicesDetail>> DeleteBulkInvoiceDetailAsync(List<InvoicesDetail> requests);

}