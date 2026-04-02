namespace Alarm112.Client.Runtime.Bots;

public sealed class BotDecisionProfile
{
    public string ProfileId { get; set; } = "bot.profile.balanced";
    public float ReactionDelaySeconds { get; set; } = 1.5f;
    public bool PreferProcedure { get; set; } = true;
    public bool CanEscalateQuickly { get; set; } = true;
}
