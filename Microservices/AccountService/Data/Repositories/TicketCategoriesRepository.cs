using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class TicketCategoriesRepository : BaseRepository, ITicketCategoriesRepository
{
    public TicketCategoriesRepository(AccountServiceDbContext context) : base(context) { }

    public Task<List<TicketCategory>> GetAllTicketsCategories()
        => Context.TicketCategories.AsNoTracking().ToListAsync();
}
