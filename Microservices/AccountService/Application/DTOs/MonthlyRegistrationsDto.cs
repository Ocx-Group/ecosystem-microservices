namespace Ecosystem.AccountService.Application.DTOs;

public class MonthlyRegistrationsDto
{
    public int Year { get; set; }
    public int Month { get; set; }
    public long Total { get; set; }
}
