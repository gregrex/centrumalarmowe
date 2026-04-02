namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class ReleaseNotesDraftController
{
    public string CurrentTitle { get; private set; } = "Alarm112 RC1 Release Notes";

    public void Load(string title) => CurrentTitle = title;
}
