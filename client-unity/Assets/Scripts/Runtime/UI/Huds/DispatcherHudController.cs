using System;
using System.Linq;

namespace Alarm112.Client.Runtime.UI.Huds;

public sealed class DispatcherHudController
{
    public string SessionId { get; private set; } = string.Empty;
    public int AvailableUnitCount { get; private set; }
    public int DispatchedUnitCount { get; private set; }
    public int PendingIncidentCount { get; private set; }
    public string PrimaryAvailableUnitId { get; private set; } = string.Empty;

    public void BindSnapshot(string json)
    {
        var snapshot = SessionSnapshotParser.Parse(json);
        SessionId = snapshot.sessionId ?? string.Empty;
        AvailableUnitCount = SessionSnapshotParser.CountUnitsByStatus(snapshot, "available");
        DispatchedUnitCount = SessionSnapshotParser.CountUnitsByStatus(snapshot, "dispatched");
        PendingIncidentCount = SessionSnapshotParser.CountIncidentsByStatus(snapshot, "pending");
        PrimaryAvailableUnitId = snapshot.units.FirstOrDefault(unit =>
            string.Equals(unit.status, "available", System.StringComparison.OrdinalIgnoreCase))?.unitId ?? string.Empty;
    }
}
