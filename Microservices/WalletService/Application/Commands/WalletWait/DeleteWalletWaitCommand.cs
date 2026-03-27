using Ecosystem.WalletService.Domain.DTOs.WalletWaitDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletWait;

public record DeleteWalletWaitCommand(int Id) : IRequest<WalletWaitDto?>;
