using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletWithdrawal;

public record CreateWalletWithdrawalCommand : IRequest<WalletWithDrawalDto>
{
    public int AffiliateId { get; init; }
    public string? AffiliateUserName { get; init; }
    public decimal Amount { get; init; }
    public string? Observation { get; init; }
    public string? AdminObservation { get; init; }
    public DateTime Date { get; init; }
    public DateTime? ResponseDate { get; init; }
    public decimal RetentionPercentage { get; init; }
    public bool IsProcessed { get; init; }
    public bool Status { get; init; }
}
