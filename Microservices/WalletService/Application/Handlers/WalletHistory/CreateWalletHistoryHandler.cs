using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletHistory;
using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletHistory;

public class CreateWalletHistoryHandler : IRequestHandler<CreateWalletHistoryCommand, WalletHistoryDto>
{
    private readonly IWalletHistoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateWalletHistoryHandler> _logger;

    public CreateWalletHistoryHandler(
        IWalletHistoryRepository repository,
        IMapper mapper,
        ILogger<CreateWalletHistoryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<WalletHistoryDto> Handle(CreateWalletHistoryCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<WalletsHistory>(request);
        var created = await _repository.CreateWalletHistoriesAsync(entity);
        return _mapper.Map<WalletHistoryDto>(created);
    }
}
