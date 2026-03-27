namespace Ecosystem.WalletService.Domain.Requests.ConPaymentRequest;

public class IpnRequest
{
    public string amount1 { get; set; }
    public string amount2 { get; set; }
    public string buyer_name { get; set; }
    public string currency1 { get; set; }
    public string currency2 { get; set; }
    public string email { get; set; }
    public string fee { get; set; }
    public string ipn_id { get; set; }
    public string ipn_mode { get; set; }
    public string ipn_type { get; set; }
    public string ipn_version { get; set; }
    public string item_name { get; set; }
    public int item_number { get; set; }
    public string merchant { get; set; }
    public int received_amount { get; set; }
    public int received_confirms { get; set; }
    public int status { get; set; }
    public string status_text { get; set; }
    public string txn_id { get; set; }
    public string custom { get; set; }
}