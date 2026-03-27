using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletWait;

public record GetWalletWaitByIdQuery(int Id) : IRequest<WalletWaitDto?>;
