using AutoMapper;
using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.Invoice;
using Ecosystem.WalletService.Domain.DTOs.InvoiceDto;
using Ecosystem.WalletService.Domain.DTOs.PaginationDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Invoice;

public class GetAllInvoicesHandler : IRequestHandler<GetAllInvoicesQuery, PaginationDto<InvoiceDto>>
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetAllInvoicesHandler> _logger;

    public GetAllInvoicesHandler(
        IInvoiceRepository invoiceRepository,
        IAccountServiceAdapter accountServiceAdapter,
        IMapper mapper,
        ITenantContext tenantContext,
        ILogger<GetAllInvoicesHandler> logger)
    {
        _invoiceRepository = invoiceRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _mapper = mapper;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<PaginationDto<InvoiceDto>> Handle(GetAllInvoicesQuery query, CancellationToken cancellationToken)
    {
        var request = query.Request;
        var brandId = _tenantContext.TenantId;

        if (request.StartDate.HasValue && request.EndDate.HasValue && request.StartDate > request.EndDate)
            throw new ArgumentException("The start date must be before the end date.");

        var response = await _invoiceRepository.GetAllInvoices(brandId, request);
        var mappedItems = _mapper.Map<List<InvoiceDto>>(response.Items);

        var userTasks = mappedItems.Select(async invoice =>
        {
            try
            {
                var user = await _accountServiceAdapter.GetUserInfo(invoice.AffiliateId, brandId);
                invoice.UserName = user?.UserName;
                invoice.Name = user?.Name;
                invoice.LastName = user?.LastName;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error getting user info for affiliate {AffiliateId}", invoice.AffiliateId);
                throw;
            }
        });

        await Task.WhenAll(userTasks);

        return new PaginationDto<InvoiceDto>
        {
            CurrentPage = response.CurrentPage,
            PageSize = response.PageSize,
            TotalCount = response.TotalCount,
            TotalPages = response.TotalPages,
            Items = mappedItems
        };
    }
}
