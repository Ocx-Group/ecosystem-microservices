using Ecosystem.WalletService.Domain.DTOs.WalletRequestDto;
using Ecosystem.WalletService.Domain.Requests.WalletRequestRequest;
using Ecosystem.WalletService.Domain.Responses;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.WalletRequest;

public record CreateWalletRequestCommand(WalletRequestRequest Request) : IRequest<ResultResponse<WalletRequestDto>>;
