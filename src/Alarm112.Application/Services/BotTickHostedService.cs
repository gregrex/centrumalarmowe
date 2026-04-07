using Alarm112.Application.Interfaces;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Alarm112.Application.Services;

/// <summary>
/// Background service that fires bot ticks every 5 seconds for all active sessions.
/// </summary>
public sealed class BotTickHostedService : BackgroundService
{
    private static readonly TimeSpan TickInterval = TimeSpan.FromSeconds(5);

    private readonly ISessionStore _store;
    private readonly IBotDirector _botDirector;
    private readonly ILogger<BotTickHostedService> _logger;

    public BotTickHostedService(ISessionStore store, IBotDirector botDirector, ILogger<BotTickHostedService> logger)
    {
        _store = store;
        _botDirector = botDirector;
        _logger = logger;
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
                catch (Exception ex)
                {
                    // Log but continue — one session failure must not crash the bot tick loop
                    _logger.LogError(ex, "BotTick failed for session {SessionId}", sessionId);
                }
            }
        }
    }
}
