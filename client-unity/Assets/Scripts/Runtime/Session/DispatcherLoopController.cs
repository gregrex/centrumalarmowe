namespace Alarm112.Client.Runtime.Session;

public sealed class DispatcherLoopController
{
    public string RecommendedAction { get; private set; } = "assign";

    public void SetRecommendedAction(string action)
    {
        RecommendedAction = action;
    }
}
