using AutoMapper;
using Ecosystem.WalletService.Application.Queries.WalletHistory;
using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using Ecosystem.WalletService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletHistory;

public class GetAllWalletHistoriesHandler : IRequestHandler<GetAllWalletHistoriesQuery, ICollection<WalletHistoryDto>>
{
    private readonly IWalletHistoryRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<GetAllWalletHistoriesHandler> _logger;

    public GetAllWalletHistoriesHandler(
        IWalletHistoryRepository repository,
        IMapper mapper,
        ILogger<GetAllWalletHistoriesHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<WalletHistoryDto>> Handle(GetAllWalletHistoriesQuery request, CancellationToken cancellationToken)
    {
        var histories = await _repository.GetAllWalletsHistoriesAsync();
        return _mapper.Map<ICollection<WalletHistoryDto>>(histories);
    }
}
