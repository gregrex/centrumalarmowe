using System;
using System.Linq;

namespace Alarm112.Client.Runtime.UI.Huds;

public sealed class CrisisOfficerHudController
{
    public string SessionId { get; private set; } = string.Empty;
    public int CriticalIncidentCount { get; private set; }
    public int BotBackfillRoleCount { get; private set; }
    public string LatestCriticalIncidentTitle { get; private set; } = string.Empty;

    public void BindSnapshot(string json)
    {
        var snapshot = SessionSnapshotParser.Parse(json);
        SessionId = snapshot.sessionId ?? string.Empty;
        CriticalIncidentCount = snapshot.incidents.Count(incident =>
            string.Equals(incident.severity, "critical", System.StringComparison.OrdinalIgnoreCase));
        BotBackfillRoleCount = SessionSnapshotParser.CountBotBackfillRoles(snapshot);
        LatestCriticalIncidentTitle = snapshot.incidents.LastOrDefault(incident =>
            string.Equals(incident.severity, "critical", System.StringComparison.OrdinalIgnoreCase))?.title ?? string.Empty;
    }
}
