using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletRetentionConfig;
using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletRetentionConfig;

public class CreateWalletRetentionConfigHandler : IRequestHandler<CreateWalletRetentionConfigCommand, IEnumerable<WalletRetentionConfigDto>>
{
    private readonly IWalletRetentionConfigRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateWalletRetentionConfigHandler> _logger;

    public CreateWalletRetentionConfigHandler(
        IWalletRetentionConfigRepository repository,
        IMapper mapper,
        ILogger<CreateWalletRetentionConfigHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<WalletRetentionConfigDto>> Handle(CreateWalletRetentionConfigCommand request, CancellationToken cancellationToken)
    {
        var results = new List<WalletRetentionConfigDto>();

        foreach (var item in request.Items)
        {
            var existing = await _repository.GetWalletRetentionConfigById(item.Id);

            if (existing is not null)
            {
                existing.WithdrawalTo = item.WithdrawalTo;
                existing.WithdrawalFrom = item.WithdrawalFrom;
                existing.DisableDate = item.DisableDate;
                existing.Percentage = item.Percentage;
                existing.Date = item.Date;
                existing.Status = item.Status;
                existing.UpdatedAt = DateTime.Now;

                await _repository.UpdateWalletRetentionConfigAsync(new List<WalletsRetentionsConfig> { existing });
                results.Add(_mapper.Map<WalletRetentionConfigDto>(existing));
            }
            else
            {
                var entity = new WalletsRetentionsConfig
                {
                    WithdrawalFrom = item.WithdrawalFrom,
                    WithdrawalTo = item.WithdrawalTo,
                    Percentage = item.Percentage,
                    DisableDate = item.DisableDate,
                    Date = item.Date,
                    Status = item.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _repository.CreateWalletRetentionConfigAsync(new List<WalletsRetentionsConfig> { entity });
                results.Add(_mapper.Map<WalletRetentionConfigDto>(entity));
            }
        }

        return results;
    }
}
