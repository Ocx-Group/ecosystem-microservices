using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletWithdrawal;
using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletWithdrawal;

public class GetWalletWithdrawalByIdHandler : IRequestHandler<GetWalletWithdrawalByIdQuery, WalletWithDrawalDto?>
{
    private readonly IWalletWithDrawalRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetWalletWithdrawalByIdHandler> _logger;

    public GetWalletWithdrawalByIdHandler(
        IWalletWithDrawalRepository repository,
        IMapper mapper,
        ILogger<GetWalletWithdrawalByIdHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletWithDrawalDto?> Handle(GetWalletWithdrawalByIdQuery request, CancellationToken cancellationToken)
    {
        var withdrawal = await _repository.GetWalletWithdrawalById(request.Id);
        return withdrawal is null ? null : _mapper.Map<WalletWithDrawalDto>(withdrawal);
    }
}
