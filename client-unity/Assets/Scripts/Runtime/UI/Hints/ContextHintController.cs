namespace Alarm112.ClientUnity.Runtime.UI.Hints;

public sealed class ContextHintController
{
    public string? ActiveHintId { get; private set; }
    public string? ActiveMessage { get; private set; }

    public void Show(string hintId, string message)
    {
        ActiveHintId = hintId;
        ActiveMessage = message;
    }

    public void Clear()
    {
        ActiveHintId = null;
        ActiveMessage = null;
    }
}
