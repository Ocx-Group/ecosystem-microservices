using Ecosystem.WalletService.Application.Adapters;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace Ecosystem.WalletService.Application.Services;

public class BrowserProvider : IBrowserProvider
{
    private IBrowser? _browser;
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly ILogger<BrowserProvider> _logger;

    public BrowserProvider(ILogger<BrowserProvider> logger)
    {
        _logger = logger;
    }

    public async Task<IBrowser> GetBrowserAsync()
    {
        if (_browser is { IsClosed: false })
            return _browser;

        await _semaphore.WaitAsync();
        try
        {
            if (_browser is { IsClosed: false })
                return _browser;

            _logger.LogInformation("Downloading Chromium for PDF generation...");
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            _logger.LogInformation("Launching Chromium browser...");
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                Args = ["--no-sandbox", "--disable-setuid-sandbox", "--disable-dev-shm-usage"]
            });

            return _browser;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
        {
            await _browser.DisposeAsync();
            _browser = null;
        }

        _semaphore.Dispose();
        GC.SuppressFinalize(this);
    }
}
