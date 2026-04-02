namespace Alarm112.Client.Runtime.Bots;

public sealed class BotSlotPresenter
{
    public string Label(string role, bool hasBot) => hasBot ? $"{role}: AI" : $"{role}: Human";
}
