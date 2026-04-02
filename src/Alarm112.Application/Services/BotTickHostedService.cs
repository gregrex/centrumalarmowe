using Alarm112.Application.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Alarm112.Application.Services;

/// <summary>
/// Background service that fires bot ticks every 5 seconds for all active sessions.
/// </summary>
public sealed class BotTickHostedService : BackgroundService
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);

    private readonly ISessionStore _store;
    private readonly IBotDirector _botDirector;

    public BotTickHostedService(ISessionStore store, IBotDirector botDirector)
    {
        _store = store;
        _botDirector = botDirector;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TickInterval, stoppingToken);

            foreach (var sessionId in _store.GetActiveSessionIds())
            {
                try
                {
                    await _botDirector.ExecuteBotTickAsync(sessionId, stoppingToken);
                }
                catch
                {
                    // Swallow per-session errors — bot tick must not crash the host
                }
            }
        }
    }
}
