namespace Ecosystem.AccountService.Domain.Models.CustomModels;

public class MonthlyRegistrations
{
    public int Year { get; set; }
    public int Month { get; set; }
    public long Total { get; set; }
}
