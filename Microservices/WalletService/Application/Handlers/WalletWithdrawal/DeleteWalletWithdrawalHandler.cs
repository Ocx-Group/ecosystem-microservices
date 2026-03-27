using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletWithdrawal;
using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWithdrawal;

public class DeleteWalletWithdrawalHandler : IRequestHandler<DeleteWalletWithdrawalCommand, WalletWithDrawalDto?>
{
    private readonly IWalletWithDrawalRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<DeleteWalletWithdrawalHandler> _logger;

    public DeleteWalletWithdrawalHandler(
        IWalletWithDrawalRepository repository,
        IMapper mapper,
        ILogger<DeleteWalletWithdrawalHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWithDrawalDto?> Handle(DeleteWalletWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.GetWalletWithdrawalById(request.Id);

        if (entity is null)
            return null;

        await _repository.DeleteWalletWithdrawalAsync(entity);
        return _mapper.Map<WalletWithDrawalDto>(entity);
    }
}
