using Ecosystem.WalletService.Domain.DTOs.PaymentTransactionDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.PaymentTransaction;

public record GetAllWireTransfersQuery : IRequest<IEnumerable<PaymentTransactionDto>>;
