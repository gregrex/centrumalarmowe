namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class StoreShotMockController
{
    public string CurrentShotKey { get; private set; } = "shot.home.hero";

    public void SelectShot(string key)
    {
        if (!string.IsNullOrWhiteSpace(key))
        {
            CurrentShotKey = key;
        }
    }
}
