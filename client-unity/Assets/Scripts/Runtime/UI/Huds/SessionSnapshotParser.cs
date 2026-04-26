using System;
using System.Linq;
using UnityEngine;

namespace Alarm112.Client.Runtime.UI.Huds;

internal static class SessionSnapshotParser
{
    public static SessionSnapshotData Parse(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("Snapshot JSON is required.", nameof(json));

        var snapshot = JsonUtility.FromJson<SessionSnapshotData>(json);
        if (snapshot == null)
            throw new InvalidOperationException("Snapshot JSON could not be parsed.");

        snapshot.incidents ??= Array.Empty<IncidentData>();
        snapshot.units ??= Array.Empty<UnitData>();
        snapshot.alerts ??= Array.Empty<AlertData>();
        snapshot.roles ??= Array.Empty<RoleData>();
        return snapshot;
    }

    [Serializable]
    internal sealed class SessionSnapshotData
    {
        public string sessionId;
        public IncidentData[] incidents;
        public UnitData[] units;
        public AlertData[] alerts;
        public RoleData[] roles;
    }

    [Serializable]
    internal sealed class IncidentData
    {
        public string incidentId;
        public string title;
        public string severity;
        public string status;
    }

    [Serializable]
    internal sealed class UnitData
    {
        public string unitId;
        public string status;
        public string kind;
        public string location;
    }

    [Serializable]
    internal sealed class AlertData
    {
        public string alertId;
        public string message;
        public string severity;
    }

    [Serializable]
    internal sealed class RoleData
    {
        public string role;
        public bool hasHuman;
        public bool hasBot;
        public string occupantId;
    }

    public static int CountIncidentsByStatus(SessionSnapshotData snapshot, string status) =>
        snapshot.incidents.Count(item => string.Equals(item.status, status, StringComparison.OrdinalIgnoreCase));

    public static int CountUnitsByStatus(SessionSnapshotData snapshot, string status) =>
        snapshot.units.Count(item => string.Equals(item.status, status, StringComparison.OrdinalIgnoreCase));

    public static int CountAlertsBySeverity(SessionSnapshotData snapshot, string severity) =>
        snapshot.alerts.Count(item => string.Equals(item.severity, severity, StringComparison.OrdinalIgnoreCase));

    public static int CountBotBackfillRoles(SessionSnapshotData snapshot) =>
        snapshot.roles.Count(item => !item.hasHuman && item.hasBot);
}
