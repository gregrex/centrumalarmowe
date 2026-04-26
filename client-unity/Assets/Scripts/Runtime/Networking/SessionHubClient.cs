using System;

namespace Alarm112.Client.Runtime.Networking;

public sealed class SessionHubClient
{
    public string State { get; private set; } = "Disconnected";
    public string ConnectedUrl { get; private set; } = string.Empty;
    public string SessionId { get; private set; } = string.Empty;
    public string LastEnvelopeJson { get; private set; } = string.Empty;
    public string LastHeartbeatAckJson { get; private set; } = string.Empty;
    public string LastError { get; private set; } = string.Empty;

    public event Action<string> EnvelopeReceived;
    public event Action<string> HeartbeatAcknowledged;

    public void Connect(string url, string sessionId)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("Hub URL is required.", nameof(url));
        if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            throw new ArgumentException("Hub URL must be absolute.", nameof(url));
        if (string.IsNullOrWhiteSpace(sessionId))
            throw new ArgumentException("SessionId is required.", nameof(sessionId));

        ConnectedUrl = url;
        SessionId = sessionId;
        LastError = string.Empty;
        State = "Connected";
    }

    public void Disconnect()
    {
        State = "Disconnected";
        ConnectedUrl = string.Empty;
        SessionId = string.Empty;
    }

    public void HandleEnvelope(string json)
    {
        EnsureConnected();
        LastEnvelopeJson = json ?? string.Empty;
        EnvelopeReceived?.Invoke(LastEnvelopeJson);
    }

    public void HandleHeartbeatAck(string json)
    {
        EnsureConnected();
        LastHeartbeatAckJson = json ?? string.Empty;
        HeartbeatAcknowledged?.Invoke(LastHeartbeatAckJson);
    }

    public void MarkFaulted(string error)
    {
        LastError = error ?? string.Empty;
        State = "Faulted";
    }

    private void EnsureConnected()
    {
        if (State != "Connected")
            throw new InvalidOperationException("Session hub client is not connected.");
    }
}
