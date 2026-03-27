using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetBalanceInformationAdminQuery : IRequest<BalanceInformationAdminDto>;
