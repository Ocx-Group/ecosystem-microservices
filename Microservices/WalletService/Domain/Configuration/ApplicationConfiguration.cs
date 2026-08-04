namespace Ecosystem.WalletService.Domain.Configuration;

public class ApplicationConfiguration
{
    public ConnectionStrings? ConnectionStrings { get; set; }
    public string? ClientUrl { get; set; }
    public EndpointTokens? EndpointTokens { get; set; }
    public Endpoints? Endpoints { get; set; }

    public EmailCredentials? EmailCredentials { get; set; }
    public ConsumersSetting? ConsumersSetting { get; set; }

    public ConPayments? ConPayments { get; set; }

    public SendingBlue? SendingBlue { get; set; }

    public CoinPay? CoinPay { get; set; }

    public Pagadito? Pagadito { get; set; }
    
    public WebTokens? WebTokens { get; set; }
}

public class ConnectionStrings
{
    public string? PostgreSqlConnection { get; set; }
    public string? RedisConnection { get; set; }
}

public class ConsumersSetting
{
    public int ConsumersCount { get; set; }
    public int ConsumersProcessPaymentCount { get; set; } = 1;
    public string BrokerList { get; set; }
}

public class Endpoints
{
    public string? WalletURL { get; set; }
    public string? AccountServiceURL { get; set; }
    public string? SystemConfigurationURL { get; set; }

    public string? InventoryServiceURL { get; set; }

    public string? CoinPayURL { get; set; }
}

public class EndpointTokens
{
    public string? WalletToken { get; set; }
    public string? AccountServiceToken { get; set; }
    public string? SystemConfigurationServiceToken { get; set; }
    public string? InventoryServiceToken { get; set; }
}

public class EmailCredentials
{
    public string? From { get; set; }
    public string? Password { get; set; }
    public string? Smtp { get; set; }
    public int Port { get; set; }
}

public class ConPayments
{
    public string Key { get; set; }
    public string Secret { get; set; }

    public string IpnSecret { get; set; }

    public string MerchantId { get; set; }

    public string DebugEmail { get; set; }
}

public class SendingBlue
{
    public string? ApiKey { get; set; }
}

public class CoinPay
{
    public string? SecretKey { get; set; }
    public string? InitialToken { get; set; }
    public int UserId { get; set; }

    /// <summary>
    /// Enforces HMAC verification of incoming webhooks. Off by default: the provider's
    /// header name and dynamic key must be confirmed with CoinPay before turning it on,
    /// otherwise every legitimate notification would be rejected.
    /// </summary>
    public bool RequireWebhookSignature { get; set; }

    /// <summary>Header carrying the HMAC-SHA512 signature of an incoming webhook.</summary>
    public string SignatureHeader { get; set; } = "X-Signature";

    /// <summary>Shared dynamic key hashed together with IdUser and IdTransaction.</summary>
    public string? DynamicKey { get; set; }
}

public class Pagadito
{
    public string? Url { get; set; }
    public string? Uid { get; set; }
    public string? Wsk { get; set; }
}

public class WebTokens
{
    public string? EcosystemToken { get; set; }
    public string? RecyCoinToken { get; set; }
    public string? HouseCoinToken { get; set; }
    
    public string? ExitoJuntosToken { get; set; }
}