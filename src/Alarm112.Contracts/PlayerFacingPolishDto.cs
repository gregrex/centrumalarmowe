namespace Alarm112.Contracts;

public sealed record PlayerFacingPolishDto(
    string VariantId,
    IReadOnlyList<string> Features,
    string QualityTarget);
