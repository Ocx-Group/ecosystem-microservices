using Ecosystem.WalletService.Domain.Models;

namespace Ecosystem.WalletService.Domain.Interfaces;

public interface IWalletRetentionConfigRepository
{
    Task<List<WalletsRetentionsConfig>> GetAllWalletsRetentionConfig();
    Task<WalletsRetentionsConfig?> GetWalletRetentionConfigById(int                                                   id);
    Task<IEnumerable<WalletsRetentionsConfig>> CreateWalletRetentionConfigAsync(IEnumerable<WalletsRetentionsConfig> request);
    Task<IEnumerable<WalletsRetentionsConfig>> UpdateWalletRetentionConfigAsync(IEnumerable<WalletsRetentionsConfig> request);
    Task<WalletsRetentionsConfig> DeleteWalletRetentionConfigAsync(WalletsRetentionsConfig                          request);
}