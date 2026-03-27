using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletRequest;
using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRequest;

public class GetAllRevertTransactionsHandler : IRequestHandler<GetAllRevertTransactionsQuery, IEnumerable<WalletRequestDto>?>
{
    private readonly IWalletRequestRepository _walletRequestRepository;
    private readonly IMapper _mapper;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GetAllRevertTransactionsHandler> _logger;

    public GetAllRevertTransactionsHandler(
        IWalletRequestRepository walletRequestRepository,
        IMapper mapper,
        ITenantContext tenantContext,
        ILogger<GetAllRevertTransactionsHandler> logger)
    {
        _walletRequestRepository = walletRequestRepository;
        _mapper = mapper;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<IEnumerable<WalletRequestDto>?> Handle(GetAllRevertTransactionsQuery request, CancellationToken cancellationToken)
    {
        var response = await _walletRequestRepository.GetAllWalletRequestRevertTransaction(_tenantContext.TenantId);
        return _mapper.Map<IEnumerable<WalletRequestDto>>(response);
    }
}
