namespace Alarm112.Application.Interfaces;

public interface IBotDirector
{
    Task ExecuteBotTickAsync(string sessionId, CancellationToken cancellationToken);
}
