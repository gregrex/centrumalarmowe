namespace Alarm112.Api.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// Real-time session hub.
/// When RequireAuth=true the hub requires an authenticated user (JWT Bearer).
/// In development (RequireAuth=false) the [Authorize] policy allows all via the permissive default policy.
/// </summary>
[Authorize]
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
