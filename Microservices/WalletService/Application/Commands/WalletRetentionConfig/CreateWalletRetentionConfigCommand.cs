using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletRetentionConfig;

public record CreateWalletRetentionConfigCommand : IRequest<IEnumerable<WalletRetentionConfigDto>>
{
    public IEnumerable<WalletRetentionConfigItem> Items { get; init; } = [];
}

public record WalletRetentionConfigItem
{
    public int Id { get; init; }
    public decimal WithdrawalFrom { get; init; }
    public decimal WithdrawalTo { get; init; }
    public decimal Percentage { get; init; }
    public DateTime Date { get; init; }
    public DateTime? DisableDate { get; init; }
    public bool Status { get; init; }
}
