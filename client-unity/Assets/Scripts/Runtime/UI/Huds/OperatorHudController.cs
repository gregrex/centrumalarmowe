using System.Linq;

namespace Alarm112.Client.Runtime.UI.Huds;

public sealed class OperatorHudController
{
    public string SessionId { get; private set; } = string.Empty;
    public int PendingIncidentCount { get; private set; }
    public int CriticalAlertCount { get; private set; }
    public string LatestAlertMessage { get; private set; } = string.Empty;

    public void BindSnapshot(string json)
    {
        var snapshot = SessionSnapshotParser.Parse(json);
        SessionId = snapshot.sessionId ?? string.Empty;
        PendingIncidentCount = SessionSnapshotParser.CountIncidentsByStatus(snapshot, "pending");
        CriticalAlertCount = SessionSnapshotParser.CountAlertsBySeverity(snapshot, "critical");
        LatestAlertMessage = snapshot.alerts.LastOrDefault()?.message ?? string.Empty;
    }
}
