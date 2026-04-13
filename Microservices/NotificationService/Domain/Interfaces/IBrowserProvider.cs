using PuppeteerSharp;

namespace Ecosystem.NotificationService.Domain.Interfaces;

/// <summary>
/// Manages a shared Chromium browser instance for PDF generation.
/// Register as Singleton in IoC.
/// </summary>
public interface IBrowserProvider : IAsyncDisposable
{
    Task<IBrowser> GetBrowserAsync();
}
