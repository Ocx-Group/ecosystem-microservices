using Ecosystem.WalletService.Domain.DTOs.PaymentTransactionDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.PaymentTransaction;

public record CreatePaymentTransactionCommand : IRequest<PaymentTransactionDto?>
{
    public string? IdTransaction { get; init; }
    public int AffiliateId { get; init; }
    public decimal Amount { get; init; }
    public string? Products { get; init; }
    public DateTime CreatedAt { get; init; }
}
