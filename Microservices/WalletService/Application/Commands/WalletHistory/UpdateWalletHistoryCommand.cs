using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletHistory;

public record UpdateWalletHistoryCommand : IRequest<WalletHistoryDto?>
{
    public int Id { get; init; }
    public int AffiliateId { get; init; }
    public int? UserId { get; init; }
    public decimal Credit { get; init; }
    public decimal Debit { get; init; }
    public decimal Deferred { get; init; }
    public bool Status { get; init; }
    public string Concept { get; init; } = null!;
    public int? Support { get; init; }
    public DateTime Date { get; init; }
}
