using Ecosystem.WalletService.Domain.DTOs.WalletRetentionConfigDto;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletRetentionConfig;

public record DeleteWalletRetentionConfigCommand(int Id) : IRequest<WalletRetentionConfigDto?>;
