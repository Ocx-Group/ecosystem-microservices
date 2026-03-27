using AutoMapper;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class GetWalletByAffiliateIdHandler : IRequestHandler<GetWalletByAffiliateIdQuery, IEnumerable<WalletDto?>>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletByAffiliateIdHandler> _logger;

    public GetWalletByAffiliateIdHandler(
        IWalletRepository walletRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetWalletByAffiliateIdHandler> logger)
    {
        _walletRepository = walletRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<WalletDto?>> Handle(GetWalletByAffiliateIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _walletRepository.GetWalletByAffiliateId(request.AffiliateId, _tenantContext.TenantId);
        return _mapper.Map<IEnumerable<WalletDto?>>(response).ToList();
    }
}
