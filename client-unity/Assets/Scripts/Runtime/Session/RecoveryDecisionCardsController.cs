namespace Alarm112.Client.Runtime.Session;

public sealed class RecoveryDecisionCardsController
{
    public string ActiveCardId { get; private set; } = "recovery.card.traffic_reroute";
    public int OptionCount { get; private set; }

    public void Bind(string cardId, int optionCount)
    {
        ActiveCardId = cardId;
        OptionCount = optionCount;
    }
}
