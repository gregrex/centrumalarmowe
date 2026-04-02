using System;
using System.Collections.Generic;

namespace Alarm112.Client.Runtime.Map;

[Serializable]
public sealed class CityMapDto
{
    public string CityId = string.Empty;
    public string PresetId = string.Empty;
    public List<CityNodeDto> Nodes = new();
    public List<CityConnectionDto> Connections = new();
}

[Serializable]
public sealed class CityNodeDto
{
    public string NodeId = string.Empty;
    public string LabelKey = string.Empty;
    public string TypeId = string.Empty;
    public float X;
    public float Y;
}

[Serializable]
public sealed class CityConnectionDto
{
    public string ConnectionId = string.Empty;
    public string FromNodeId = string.Empty;
    public string ToNodeId = string.Empty;
    public string TypeId = string.Empty;
}

[Serializable]
public sealed class SessionTimelineItemDto
{
    public string TimelineItemId = string.Empty;
    public string Severity = string.Empty;
    public string ActorRole = string.Empty;
    public string Message = string.Empty;
    public bool IsBot;
}
