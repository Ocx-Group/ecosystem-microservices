using Ecosystem.ConfigurationService.Data.Context;

namespace Ecosystem.ConfigurationService.Data.Repositories;

public class BaseRepository
{
    protected readonly ConfigurationServiceDbContext Context;

    protected BaseRepository(ConfigurationServiceDbContext context)
        => Context = context;
}
