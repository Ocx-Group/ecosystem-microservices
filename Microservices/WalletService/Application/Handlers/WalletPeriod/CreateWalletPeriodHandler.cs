using AutoMapper;
using Ecosystem.WalletService.Application.Commands.WalletPeriod;
using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using Ecosystem.WalletService.Domain.Interfaces;
using Ecosystem.WalletService.Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Ecosystem.WalletService.Application.Handlers.WalletPeriod;

public class CreateWalletPeriodHandler : IRequestHandler<CreateWalletPeriodCommand, IEnumerable<WalletPeriodDto>>
{
    private readonly IWalletPeriodRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateWalletPeriodHandler> _logger;

    public CreateWalletPeriodHandler(
        IWalletPeriodRepository repository,
        IMapper mapper,
        ILogger<CreateWalletPeriodHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<WalletPeriodDto>> Handle(CreateWalletPeriodCommand request, CancellationToken cancellationToken)
    {
        var results = new List<WalletPeriodDto>();

        foreach (var item in request.Items)
        {
            var existing = await _repository.GetWalletPeriodById(item.Id);

            if (existing is not null)
            {
                existing.Date = item.Date;
                existing.Status = item.Status;
                existing.UpdatedAt = DateTime.Now;

                await _repository.UpdateWalletPeriodsAsync(new List<WalletsPeriod> { existing });
                results.Add(_mapper.Map<WalletPeriodDto>(existing));
            }
            else
            {
                var entity = new WalletsPeriod
                {
                    Date = item.Date,
                    Status = item.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                await _repository.CreateWalletPeriodAsync(new List<WalletsPeriod> { entity });
                results.Add(_mapper.Map<WalletPeriodDto>(entity));
            }
        }

        return results;
    }
}
