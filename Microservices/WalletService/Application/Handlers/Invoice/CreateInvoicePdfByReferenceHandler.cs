using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class CreateInvoicePdfByReferenceHandler : IRequestHandler<CreateInvoicePdfByReferenceQuery, InvoiceResultDto?>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IPdfService _pdfService;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<CreateInvoicePdfByReferenceHandler> _logger;

    public CreateInvoicePdfByReferenceHandler(
        IInvoiceRepository invoiceRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IPdfService pdfService,
        ITenantContext tenantContext,
        ILogger<CreateInvoicePdfByReferenceHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _pdfService = pdfService;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<InvoiceResultDto?> Handle(CreateInvoicePdfByReferenceQuery request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var invoice = await _invoiceRepository.GetInvoiceByReceiptNumber(request.Reference, brandId);

        if (invoice is null)
            return null;

        var user = await _accountServiceAdapter.GetUserInfo(invoice.AffiliateId, brandId);

        if (user is null)
            return null;

        var generatedInvoice = await _pdfService.RegenerateInvoice(user, invoice);

        return new InvoiceResultDto
        {
            PdfContent = generatedInvoice,
            BrandId = brandId
        };
    }
}
