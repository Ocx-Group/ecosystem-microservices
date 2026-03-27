namespace Ecosystem.WalletService.Domain.Requests.PaginationRequest;

public class PaginationRequest
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;
    
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
    public int PageNumber { get; set; } = 1;
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}