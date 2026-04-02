namespace Alarm112.Contracts;

public sealed record MissionFailBranchDto(
    string Id,
    string Type,
    string Title,
    string RetryHint);
