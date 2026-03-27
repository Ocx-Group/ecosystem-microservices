using AutoMapper;
using Ecosystem.WalletService.Application.Queries.Wallet;
using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class GetWalletByIdHandler : IRequestHandler<GetWalletByIdQuery, WalletDto?>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletByIdHandler> _logger;

    public GetWalletByIdHandler(
        IWalletRepository walletRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<GetWalletByIdHandler> logger)
    {
        _walletRepository = walletRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletDto?> Handle(GetWalletByIdQuery request, CancellationToken cancellationToken)
    {
        var response = await _walletRepository.GetWalletById(request.Id, _tenantContext.TenantId);
        return _mapper.Map<WalletDto>(response);
    }
}
