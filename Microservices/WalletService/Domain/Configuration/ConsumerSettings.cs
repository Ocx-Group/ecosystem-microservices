namespace Ecosystem.WalletService.Domain.Configuration;

public class ConsumerSettings
{
    public string[] Topics { get; set; }
    public string GroupId { get; set; }
    public bool ReadLast { get; set; } = false;
    public bool UniqueByMac { get; set; } = true;
    public string GroupInstanceId { get; set; }
    public bool EnableAutoCommit { get; set; } = false;
}