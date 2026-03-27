using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletWithdrawal;
using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWithdrawal;

public class GetAllWalletWithdrawalsHandler : IRequestHandler<GetAllWalletWithdrawalsQuery, ICollection<WalletWithDrawalDto>>
{
    private readonly IWalletWithDrawalRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllWalletWithdrawalsHandler> _logger;

    public GetAllWalletWithdrawalsHandler(
        IWalletWithDrawalRepository repository,
        IMapper mapper,
        ILogger<GetAllWalletWithdrawalsHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<WalletWithDrawalDto>> Handle(GetAllWalletWithdrawalsQuery request, CancellationToken cancellationToken)
    {
        var withdrawals = await _repository.GetAllWalletsWithdrawals();
        return _mapper.Map<ICollection<WalletWithDrawalDto>>(withdrawals);
    }
}
