namespace Ecosystem.AccountService.Application.DTOs.Ticket;

public class TicketSummaryDto
{
    public long TicketId { get; set; }
    public string Title { get; set; } = null!;
    public int UnreadMessagesCount { get; set; }
    public DateTime LastUpdated { get; set; }
}
