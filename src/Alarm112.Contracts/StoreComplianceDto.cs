namespace Alarm112.Contracts;

public sealed record StoreComplianceDto(
    string Platform,
    IReadOnlyList<StoreComplianceItemDto> Items,
    string Notes);
