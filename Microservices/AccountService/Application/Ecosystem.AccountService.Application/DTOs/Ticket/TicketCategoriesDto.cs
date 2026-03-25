namespace Ecosystem.AccountService.Application.DTOs.Ticket;

public class TicketCategoriesDto
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }
}
