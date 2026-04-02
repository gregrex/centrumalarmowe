namespace Alarm112.Contracts;

public sealed record PlaytestFeedbackFieldDto(
    string Key,
    string Label,
    string FieldType,
    bool Required);
