using Ecosystem.WalletService.Data.Context;

namespace Ecosystem.WalletService.Data.Repositories;

public class BaseRepository
{
    protected readonly WalletServiceDbContext Context;

    protected BaseRepository(WalletServiceDbContext context)
        => Context = context;
}