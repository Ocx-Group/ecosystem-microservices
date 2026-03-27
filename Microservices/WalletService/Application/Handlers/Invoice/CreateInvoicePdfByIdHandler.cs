using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class CreateInvoicePdfByIdHandler : IRequestHandler<CreateInvoicePdfByIdQuery, byte[]>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IPdfService _pdfService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateInvoicePdfByIdHandler> _logger;

    public CreateInvoicePdfByIdHandler(
        IInvoiceRepository invoiceRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IPdfService pdfService,
        ITenantContext tenantContext,
        ILogger<CreateInvoicePdfByIdHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _pdfService = pdfService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<byte[]> Handle(CreateInvoicePdfByIdQuery request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var invoice = await _invoiceRepository.GetInvoiceById(request.InvoiceId, brandId);

        if (invoice is null)
            return Array.Empty<byte>();

        var user = await _accountServiceAdapter.GetUserInfo(invoice.AffiliateId, brandId);

        if (user is null)
            return Array.Empty<byte>();

        var generatedInvoice = await _pdfService.RegenerateInvoice(user, invoice);
        return generatedInvoice;
    }
}
