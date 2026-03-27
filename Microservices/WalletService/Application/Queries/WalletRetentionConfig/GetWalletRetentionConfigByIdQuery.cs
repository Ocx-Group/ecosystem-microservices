using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Queries.WalletRetentionConfig;

public record GetWalletRetentionConfigByIdQuery(int Id) : IRequest<WalletRetentionConfigDto?>;
