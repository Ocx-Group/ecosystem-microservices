using Ecosystem.WalletService.Domain.Requests.TransferBalanceRequest;
using MediatR;

namespace Ecosystem.WalletService.Application.Commands.Wallet;

public record TransferBalanceForNewAffiliateCommand(TransferBalanceRequest Request) : IRequest<bool>;
