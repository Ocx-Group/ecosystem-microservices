using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletWait;

public record GetAllWalletWaitsQuery : IRequest<ICollection<WalletWaitDto>>;
