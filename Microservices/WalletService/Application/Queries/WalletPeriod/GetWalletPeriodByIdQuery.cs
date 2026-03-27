using Ecosystem.WalletService.Domain.DTOs.WalletPeriodDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletPeriod;

public record GetWalletPeriodByIdQuery(int Id) : IRequest<WalletPeriodDto?>;
