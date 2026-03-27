using Ecosystem.WalletService.Domain.DTOs.WalletWithDrawalDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletWithdrawal;

public record UpdateWalletWithdrawalCommand : IRequest<WalletWithDrawalDto?>
{
    public int Id { get; init; }
    public int AffiliateId { get; init; }
    public decimal Amount { get; init; }
    public string? Observation { get; init; }
    public string? AdminObservation { get; init; }
    public DateTime Date { get; init; }
    public DateTime? ResponseDate { get; init; }
    public decimal RetentionPercentage { get; init; }
    public bool Status { get; init; }
}
