namespace Alarm112.ClientUnity.Runtime.UI.Hints;

public sealed class ReleaseReadinessChecklistController
{
    public int CompletedCount { get; private set; }

    public void SetCompletedCount(int value)
    {
        CompletedCount = value;
    }
}
