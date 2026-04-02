namespace Alarm112.Contracts;

public sealed record ProfileCosmeticDto(
    string CosmeticId,
    string Category,
    string Rarity,
    string UnlockSource);
