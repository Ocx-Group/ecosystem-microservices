using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletPeriod;

public record DeleteWalletPeriodCommand(int Id) : IRequest<WalletPeriodDto?>;
