namespace Alarm112.ClientUnity.Runtime.UI.Hints;

public sealed class BugBashChecklistController
{
    public int CheckedItems { get; private set; }

    public void MarkItemPassed() => CheckedItems++;
}
