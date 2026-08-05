using Ecosystem.WalletService.Domain.Constants;

namespace Ecosystem.WalletService.Api.Middlewares;

/// <summary>
/// Makes the request body rewindable on the CoinPayments notification routes. Form binding
/// consumes the stream before the action runs, and the provider's HMAC is taken over the raw
/// bytes, so without buffering the digest could never be verified.
/// </summary>
public class IpnBufferingMiddleware
{
    private static readonly string[] BufferedPaths =
    [
        CoinPaymentsConstants.IpnPath,
        CoinPaymentsConstants.MatrixIpnPath
    ];

    private readonly RequestDelegate _next;

    public IpnBufferingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value ?? string.Empty;

        if (BufferedPaths.Any(buffered => path.StartsWith(buffered, StringComparison.OrdinalIgnoreCase)))
            context.Request.EnableBuffering();

        await _next(context);
    }
}
