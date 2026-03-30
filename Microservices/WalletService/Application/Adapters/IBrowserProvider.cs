using PuppeteerSharp;

namespace Ecosystem.WalletService.Application.Adapters;

/// <summary>
/// Manages a shared Chromium browser instance for PDF generation.
/// Register as Singleton in IoC.
/// </summary>
public interface IBrowserProvider : IAsyncDisposable
{
    Task<IBrowser> GetBrowserAsync();
}
