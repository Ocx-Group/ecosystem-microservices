using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IWalletWithDrawalRepository
{
    Task<List<WalletsWithdrawal>> GetAllWalletsWithdrawals();
    Task<WalletsWithdrawal?> GetWalletWithdrawalById(int                   id);
    Task<WalletsWithdrawal> CreateWalletWithdrawalAsync(WalletsWithdrawal request);
    Task<WalletsWithdrawal> UpdateWalletWithdrawalAsync(WalletsWithdrawal request);
    Task<WalletsWithdrawal> DeleteWalletWithdrawalAsync(WalletsWithdrawal request);
}