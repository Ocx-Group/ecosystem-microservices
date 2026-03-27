using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IWalletWaitRepository
{
    Task<List<WalletsWait>> GetAllWalletsWaits();
    Task<WalletsWait?> GetWalletWaitById(int             id);
    Task<WalletsWait> CreateWalletWaitAsync(WalletsWait request);
    Task<WalletsWait> UpdateWalletWaitAsync(WalletsWait request);
    Task<WalletsWait> DeleteWalletWaitAsync(WalletsWait request);
}