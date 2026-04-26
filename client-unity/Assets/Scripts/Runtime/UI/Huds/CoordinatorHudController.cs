using System;
using System.Linq;

namespace Alarm112.Client.Runtime.UI.Huds;

public sealed class CoordinatorHudController
{
    public string SessionId { get; private set; } = string.Empty;
    public int EscalatedIncidentCount { get; private set; }
    public int ResolvedIncidentCount { get; private set; }
    public int CriticalIncidentCount { get; private set; }

    public void BindSnapshot(string json)
    {
        var snapshot = SessionSnapshotParser.Parse(json);
        SessionId = snapshot.sessionId ?? string.Empty;
        EscalatedIncidentCount = SessionSnapshotParser.CountIncidentsByStatus(snapshot, "escalated");
        ResolvedIncidentCount = SessionSnapshotParser.CountIncidentsByStatus(snapshot, "resolved");
        CriticalIncidentCount = snapshot.incidents.Count(incident =>
            string.Equals(incident.severity, "critical", System.StringComparison.OrdinalIgnoreCase));
    }
}
