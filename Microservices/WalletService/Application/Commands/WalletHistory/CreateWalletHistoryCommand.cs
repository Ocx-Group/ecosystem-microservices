using Ecosystem.WalletService.Domain.DTOs.WalletHistoryDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletHistory;

public record CreateWalletHistoryCommand : IRequest<WalletHistoryDto>
{
    public int AffiliateId { get; init; }
    public int? UserId { get; init; }
    public decimal Credit { get; init; }
    public decimal Debit { get; init; }
    public decimal Deferred { get; init; }
    public bool Status { get; init; }
    public string Concept { get; init; } = null!;
    public int? Support { get; init; }
    public DateTime Date { get; init; }
    public bool Compression { get; init; }
}
