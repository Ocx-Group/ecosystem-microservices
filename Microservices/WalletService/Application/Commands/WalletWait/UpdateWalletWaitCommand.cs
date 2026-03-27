using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletWait;

public record UpdateWalletWaitCommand : IRequest<WalletWaitDto?>
{
    public int Id { get; init; }
    public int AffiliateId { get; init; }
    public decimal? Credit { get; init; }
    public string? PaymentMethod { get; init; }
    public string? Bank { get; init; }
    public string? Support { get; init; }
    public DateTime? DepositDate { get; init; }
    public bool? Status { get; init; }
    public bool? Attended { get; init; }
    public DateTime Date { get; init; }
    public string? Order { get; init; }
}
