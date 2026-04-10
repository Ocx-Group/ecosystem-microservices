using Ecosystem.WalletService.Application.Adapters;
using Ecosystem.WalletService.Application.Queries.PaymentTransaction;
using Ecosystem.WalletService.Domain.DTOs.PaymentTransactionDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.PaymentTransaction;

public class GetAllWireTransfersHandler : IRequestHandler<GetAllWireTransfersQuery, IEnumerable<PaymentTransactionDto>>
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly IAccountServiceAdapter _accountServiceAdapter;
    private readonly ITenantContext _tenantContext;

    public GetAllWireTransfersHandler(
        ITransactionRepository transactionRepository,
        IAccountServiceAdapter accountServiceAdapter,
        ITenantContext tenantContext,
        ILogger<GetAllWireTransfersHandler> logger)
    {
        _transactionRepository = transactionRepository;
        _accountServiceAdapter = accountServiceAdapter;
        _tenantContext = tenantContext;
    }

    public async Task<IEnumerable<PaymentTransactionDto>> Handle(GetAllWireTransfersQuery request, CancellationToken cancellationToken)
    {
        var brandId = _tenantContext.TenantId;
        var transactions = await _transactionRepository.GetAllWireTransfer(brandId);
        var uniqueAffiliateIds = transactions.Select(t => t.AffiliateId).Distinct();

        var userTasks = uniqueAffiliateIds.Select(id => _accountServiceAdapter.GetUserInfo(id, brandId));
        var userResponses = await Task.WhenAll(userTasks);

        var usersInfo = userResponses
            .Where(u => u != null)
            .ToDictionary(u => u!.Id, u => u);

        if (!usersInfo.Any())
            return Enumerable.Empty<PaymentTransactionDto>();

        return transactions.Select(t => new PaymentTransactionDto
        {
            Id = t.Id,
            IdTransaction = t.IdTransaction,
            AffiliateId = t.AffiliateId,
            Amount = t.Amount,
            AmountReceived = t.AmountReceived,
            Products = t.Products,
            Status = t.Status,
            Acredited = t.Acredited,
            CreatedAt = t.CreatedAt,
            UpdatedAt = t.UpdatedAt,
            DeletedAt = t.DeletedAt,
            PaymentMethod = t.PaymentMethod,
            UserName = usersInfo.TryGetValue(t.AffiliateId, out var v) ? v?.UserName : null
        });
    }
}
