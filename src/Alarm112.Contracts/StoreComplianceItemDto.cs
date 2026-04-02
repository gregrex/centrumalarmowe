namespace Alarm112.Contracts;

public sealed record StoreComplianceItemDto(
    string Id,
    string Title,
    string Status,
    string Owner,
    string Severity);
