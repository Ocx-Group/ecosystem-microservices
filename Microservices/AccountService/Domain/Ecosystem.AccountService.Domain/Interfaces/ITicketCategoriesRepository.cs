using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface ITicketCategoriesRepository
{
    Task<List<TicketCategory>> GetAllTicketsCategories();
}
