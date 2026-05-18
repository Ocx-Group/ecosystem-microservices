using Ecosystem.AccountService.Data.Context;
using Ecosystem.AccountService.Domain.Interfaces;
using Ecosystem.AccountService.Domain.Models;
using Ecosystem.AccountService.Domain.Models.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace Ecosystem.AccountService.Data.Repositories;

public class TicketRepository : BaseRepository, ITicketRepository
{
    public TicketRepository(AccountServiceDbContext context) : base(context) { }

    public async Task<Ticket> CreateTicket(Ticket ticket, long brandId)
    {
        var today = DateTime.Now;
        ticket.CreatedAt = today;
        ticket.UpdatedAt = today;
        ticket.Status = true;
        ticket.BrandId = brandId;
        await Context.Tickets.AddAsync(ticket);
        await Context.SaveChangesAsync();
        return ticket;
    }

    public Task<List<Ticket>> GetAllTicketsByAffiliateId(int affiliateId, long brandId)
        => Context.Tickets.Where(e => e.AffiliateId == affiliateId && e.BrandId == brandId).Include(x => x.TicketImages)
            .AsNoTracking().ToListAsync();

    public Task<List<Ticket>> GetTicketsByBrandId(int brandId)
        => Context.Tickets
            .Where(x => x.BrandId == brandId)
            .AsNoTracking()
            .ToListAsync();

    public async Task<Ticket?> GetTicketById(int ticketId)
    {
        var ticket = await Context.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId && t.DeletedAt == null);
        if (ticket == null) return null;

        var messages = await GetTicketMessagesByTicketId(ticketId);
        var ticketMessages = messages
            .Where(m => m != null)
            .Select(m => new TicketMessage { Id = m.Id, MessageContent = m.MessageContent, CreatedAt = m.CreatedAt })
            .ToList();
        ticket.TicketMessages = ticketMessages;
        return ticket;
    }

    public async Task<List<Ticket>> GetAllTickets(int brandId)
    {
        var tickets = await Context.Tickets
            .Where(x => x.BrandId == brandId)
            .Include(x => x.TicketImages)
            .AsNoTracking()
            .ToListAsync();

        if (!tickets.Any()) return new List<Ticket>();

        foreach (var ticket in tickets)
        {
            var messages = await GetTicketMessagesByTicketId(ticket.Id);
            var ticketMessages = messages
                .Where(m => m != null)
                .Select(m => new TicketMessage { Id = m.Id, MessageContent = m.MessageContent, CreatedAt = m.CreatedAt })
                .ToList();
            ticket.TicketMessages = ticketMessages;
        }
        return tickets;
    }

    public async Task<List<Ticket>> DeleteTickets(List<Ticket> tickets)
    {
        var today = DateTime.Now;
        foreach (var ticket in tickets)
        {
            ticket.Status = false;
            ticket.DeletedAt = today;
        }
        Context.Tickets.UpdateRange(tickets);
        await Context.SaveChangesAsync();
        return tickets;
    }

    private async Task<ICollection<MessageDetails>> GetTicketMessagesByTicketId(long ticketId)
    {
        var messages =
            from ticketMessage in Context.TicketMessages.AsNoTracking()
            join affiliate in Context.UsersAffiliates.AsNoTracking()
                on ticketMessage.UserId equals affiliate.Id into affiliateJoin
            from affiliate in affiliateJoin.DefaultIfEmpty()
            join user in Context.Users.AsNoTracking()
                on ticketMessage.UserId equals user.Id into userJoin
            from user in userJoin.DefaultIfEmpty()
            where ticketMessage.TicketId == ticketId
            orderby ticketMessage.Id
            select new MessageDetails
            {
                Id = (int)ticketMessage.Id,
                TicketId = (int)ticketMessage.TicketId,
                UserId = ticketMessage.UserId,
                MessageContent = ticketMessage.MessageContent,
                CreatedAt = ticketMessage.CreatedAt,
                UpdatedAt = ticketMessage.UpdatedAt,
                DeletedAt = ticketMessage.DeletedAt,
                IsRead = ticketMessage.IsRead,
                UserName = affiliate != null ? affiliate.Username : user != null ? user.Username : string.Empty,
                ImageProfileUrl = affiliate != null ? affiliate.ImageProfileUrl : user != null ? user.ImageProfileUrl : string.Empty
            };

        return await messages.ToListAsync();
    }

    public async Task<List<TicketMessage>?> GetMessagesByTicketId(int ticketId)
        => await Context.TicketMessages.Where(x => x.TicketId == ticketId).AsNoTracking().ToListAsync();

    public async Task<List<TicketImage>?> GetImagesByTicketId(int ticketId)
        => await Context.TicketImages.Where(x => x.TicketId == ticketId).AsNoTracking().ToListAsync();

    public async Task<List<TicketMessage>?> DeleteMessages(List<TicketMessage> ticketMessages)
    {
        var today = DateTime.Now;
        foreach (var ticket in ticketMessages) ticket.DeletedAt = today;
        Context.TicketMessages.UpdateRange(ticketMessages);
        await Context.SaveChangesAsync();
        return ticketMessages;
    }

    public async Task<List<TicketImage>?> DeleteImages(List<TicketImage> ticketImages)
    {
        var today = DateTime.Now;
        foreach (var ticket in ticketImages) ticket.DeletedAt = today;
        Context.TicketImages.UpdateRange(ticketImages);
        await Context.SaveChangesAsync();
        return ticketImages;
    }
}
