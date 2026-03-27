using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface ICreditRepository
{
    Task<Credit> CreateCredit(Credit credit);
}