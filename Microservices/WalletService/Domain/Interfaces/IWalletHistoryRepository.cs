using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IWalletHistoryRepository
{
    Task<List<WalletsHistory>> GetAllWalletsHistoriesAsync();
    Task<WalletsHistory?> GetWalletHistoriesByIdAsync(int            id);
    Task<WalletsHistory> CreateWalletHistoriesAsync(WalletsHistory request);
    Task<WalletsHistory> UpdateWalletHistoriesAsync(WalletsHistory request);
    Task<WalletsHistory> DeleteWalletHistoriesAsync(WalletsHistory request);
}