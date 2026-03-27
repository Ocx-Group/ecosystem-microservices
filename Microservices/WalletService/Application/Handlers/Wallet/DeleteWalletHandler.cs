using AutoMapper;
using Ecosystem.WalletService.Application.Commands.Wallet;
using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.Domain.Core.MultiTenancy;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.Wallet;

public class DeleteWalletHandler : IRequestHandler<DeleteWalletCommand, WalletDto?>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ITenantContext _tenantContext;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteWalletHandler> _logger;

    public DeleteWalletHandler(
        IWalletRepository walletRepository,
        ITenantContext tenantContext,
        IMapper mapper,
        ILogger<DeleteWalletHandler> logger)
    {
        _walletRepository = walletRepository;
        _tenantContext = tenantContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletDto?> Handle(DeleteWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetWalletById(request.Id, _tenantContext.TenantId);

        if (wallet is null)
            return null;

        await _walletRepository.DeleteWalletAsync(wallet);

        return _mapper.Map<WalletDto>(wallet);
    }
}
