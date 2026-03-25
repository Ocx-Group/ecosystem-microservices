namespace Ecosystem.AccountService.Application.DTOs.Ticket;

public class TicketImagesDto
{
    public int Id { get; set; }
    public int TicketId { get; set; }
    public string ImagePath { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
