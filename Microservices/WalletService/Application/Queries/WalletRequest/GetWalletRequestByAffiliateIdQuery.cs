using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletRequest;

public record GetWalletRequestByAffiliateIdQuery(int Id) : IRequest<IEnumerable<WalletRequestDto>?>;
