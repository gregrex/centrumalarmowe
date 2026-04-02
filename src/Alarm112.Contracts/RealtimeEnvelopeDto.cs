namespace Alarm112.Contracts;

public sealed record RealtimeEnvelopeDto(
    string MessageId,
    string SessionId,
    string MessageType,
    DateTimeOffset IssuedAtUtc,
    string SenderRole,
    string? CorrelationId,
    string SchemaVersion,
    object Payload);
