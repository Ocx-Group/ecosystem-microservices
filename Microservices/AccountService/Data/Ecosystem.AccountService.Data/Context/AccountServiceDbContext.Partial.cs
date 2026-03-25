using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Context;

public partial class AccountServiceDbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
        // Extend or override model configuration here without touching the scaffolded file.
    }
}

