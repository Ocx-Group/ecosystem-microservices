namespace Ecosystem.WalletService.Domain.Constants;

public static class MonthlyCommissionConstants
{
    /// <summary>
    /// Route prefix of the liquidation endpoints; excluded from tenant resolution
    /// because they authenticate with an admin JWT rather than an X-Client-ID header.
    /// Lower case because the skip prefixes are matched case-insensitively against the
    /// request path and this is the shape ASP.NET produces for the controller.
    /// </summary>
    public const string RoutePrefix = "/api/v1/monthlycommission";

    /// <summary>
    /// Longest period a single run may cover. The screen liquidates one calendar month,
    /// and the proration divides by the number of days in the range, so a wider range
    /// would quietly change what every invoice earns.
    /// </summary>
    public const int MaxPeriodDays = 62;
}
