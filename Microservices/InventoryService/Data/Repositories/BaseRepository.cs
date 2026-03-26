using Ecosystem.InventoryService.Data.Context;

namespace Ecosystem.InventoryService.Data.Repositories;

public class BaseRepository
{
    protected readonly InventoryServiceDbContext Context;
    protected BaseRepository(InventoryServiceDbContext context) => Context = context;
}
