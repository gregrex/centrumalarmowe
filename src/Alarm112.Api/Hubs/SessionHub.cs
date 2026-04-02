namespace Alarm112.Api.Hubs;

using Microsoft.AspNetCore.SignalR;

public sealed class SessionHub : Hub
{
    public async Task JoinSession(string sessionId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, sessionId);
        await Clients.Caller.SendAsync("session.joined", sessionId);
    }

    public async Task LeaveSession(string sessionId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, sessionId);
        await Clients.Caller.SendAsync("session.left", sessionId);
    }

    public Task Heartbeat(string sessionId, string role)
    {
        return Clients.Caller.SendAsync("session.heartbeat.ack", new { sessionId, role, utc = DateTimeOffset.UtcNow });
    }
}
