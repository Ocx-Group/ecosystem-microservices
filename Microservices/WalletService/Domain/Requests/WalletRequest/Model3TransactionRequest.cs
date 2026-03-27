namespace Ecosystem.WalletService.Domain.Requests.WalletRequest;

public class Model3TransactionRequest
{
    public int Case { get; set; }
    public decimal Percentage { get; set; }
    public int EcoPoolConfigurationId { get; set; }
    public double TotalPercentageLevels { get; set; }
    public ICollection<Model3Type> EcoPoolsType { get; set; }
    public ICollection<Model3LevelsType> LevelsType { get; set; }
    
}

public class Model3LevelsType
{
    public int Level { get; set; }
    public double Percentage { get; set; }
    public int AffiliateId { get; set; }
    public string AffiliateName { get; set; }
    public int PoolId { get; set; }
    public int Side { get; set; }
    public DateTime? UserCreatedAt { get; set; }
    
}

public class Model3Type
{
    public int AffiliateId { get; set; }
    public int ProductExternalId { get; set; }
    public string AffiliateUserName { get; set; }
    public string ProductName { get; set; }
    public int CountDays { get; set; }
    public int DaysInMonth { get; set; }
    public decimal Amount { get; set; }
    public DateTime LastDayDate { get; set; }
    public DateTime PaymentDate { get; set; }
    public int PoolId { get; set; }
    public DateTime? UserCreatedAt { get; set; }
}