namespace Alarm112.Client.Runtime.Networking;

public sealed class SessionHubClient
{
    public string State => "Disconnected";

    public void Connect(string url, string sessionId)
    {
        // TODO: integrate SignalR client for Unity.
    }
}
