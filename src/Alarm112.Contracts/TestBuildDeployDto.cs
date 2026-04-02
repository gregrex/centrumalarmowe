namespace Alarm112.Contracts;

public sealed record TestBuildDeployDto(
    string BuildId,
    string Environment,
    string ClientChannel,
    string ApiBaseUrl,
    IReadOnlyList<string> RequiredChecks,
    bool ReadyForInternalReview);
