using Ecosystem.WalletService.Domain.DTOs.WalletDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.Wallet;

public record GetWalletByIdQuery(int Id) : IRequest<WalletDto?>;
