using Ecosystem.AccountService.Domain.Models;

namespace Ecosystem.AccountService.Domain.Interfaces;

public interface ITicketMessageRepository
{
    Task<TicketMessage> CreateTicketMessage(TicketMessage request);
    Task<List<TicketMessage>> GetMessagesByTicketId(long ticketId);
    Task<List<TicketMessage>> UpdateMessagesByTicketId(int ticketId);
}
