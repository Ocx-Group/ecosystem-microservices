using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class TicketMessageRepository : BaseRepository, ITicketMessageRepository
{
    public TicketMessageRepository(AccountServiceDbContext context) : base(context) { }

    public async Task<TicketMessage> CreateTicketMessage(TicketMessage request)
    {
        var today = DateTime.Now;
        request.CreatedAt = today;
        request.UpdatedAt = today;
        await Context.TicketMessages.AddAsync(request);
        await Context.SaveChangesAsync();
        return request;
    }

    public async Task<List<TicketMessage>> GetMessagesByTicketId(long ticketId)
        => await Context.TicketMessages.Where(x => x.TicketId == ticketId).AsNoTracking().ToListAsync();

    public async Task<List<TicketMessage>> UpdateMessagesByTicketId(int ticketId)
    {
        var messages = await Context.TicketMessages.Where(x => x.TicketId == ticketId).ToListAsync();
        foreach (var message in messages)
        {
            message.IsRead = true;
            message.UpdatedAt = DateTime.Now;
        }
        await Context.SaveChangesAsync();
        return messages;
    }
}
