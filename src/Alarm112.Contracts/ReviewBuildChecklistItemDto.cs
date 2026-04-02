namespace Alarm112.Contracts;

public sealed record ReviewBuildChecklistItemDto(
    string Key,
    string Title,
    string Severity,
    bool Required,
    bool Passed);
