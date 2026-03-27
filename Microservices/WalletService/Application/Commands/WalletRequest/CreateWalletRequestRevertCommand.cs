using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequestRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletRequest;

public record CreateWalletRequestRevertCommand(WalletRequestRevertTransaction Request) : IRequest<WalletRequestDto?>;
