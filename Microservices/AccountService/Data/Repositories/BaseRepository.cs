using Ecosystem.AccountService.Data.Context;

namespace Ecosystem.AccountService.Data.Repositories;

public class BaseRepository
{
    protected readonly AccountServiceDbContext Context;

    protected BaseRepository(AccountServiceDbContext context)
        => Context = context;
}
