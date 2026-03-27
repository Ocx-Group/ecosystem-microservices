using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletPeriod;

public record CreateWalletPeriodCommand : IRequest<IEnumerable<WalletPeriodDto>>
{
    public IEnumerable<WalletPeriodItem> Items { get; init; } = [];
}

public record WalletPeriodItem
{
    public int Id { get; init; }
    public DateOnly Date { get; init; }
    public bool Status { get; init; }
}
