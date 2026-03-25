using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface ITicketRepository
{
    Task<Ticket> CreateTicket(Ticket ticket, long brandId);
    Task<List<Ticket>> GetAllTicketsByAffiliateId(int affiliateId, long brandId);
    Task<List<Ticket>> GetAllTickets(int brandId);
    Task<Ticket?> GetTicketById(int ticketId);
    Task<List<Ticket>> DeleteTickets(List<Ticket> tickets);
    Task<List<TicketImage>?> DeleteImages(List<TicketImage> ticketImages);
    Task<List<TicketMessage>?> DeleteMessages(List<TicketMessage> ticketMessages);
    Task<List<TicketMessage>?> GetMessagesByTicketId(int ticketId);
    Task<List<TicketImage>?> GetImagesByTicketId(int ticketId);
}
