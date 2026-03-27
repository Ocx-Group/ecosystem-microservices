using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletWithdrawal;
using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWithdrawal;

public class CreateWalletWithdrawalHandler : IRequestHandler<CreateWalletWithdrawalCommand, WalletWithDrawalDto>
{
    private readonly IWalletWithDrawalRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateWalletWithdrawalHandler> _logger;

    public CreateWalletWithdrawalHandler(
        IWalletWithDrawalRepository repository,
        IMapper mapper,
        ILogger<CreateWalletWithdrawalHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWithDrawalDto> Handle(CreateWalletWithdrawalCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<WalletsWithdrawal>(request);
        var created = await _repository.CreateWalletWithdrawalAsync(entity);
        return _mapper.Map<WalletWithDrawalDto>(created);
    }
}
