using UnityEngine;

namespace Alarm112.Client.Runtime.UI.Theme;

[CreateAssetMenu(menuName = "Alarm112/UI/QualityThemeConfig")]
public sealed class QualityThemeConfig : ScriptableObject
{
    public Color OperatorAccent = new(0.18f, 0.85f, 0.95f);
    public Color DispatcherAccent = new(0.25f, 0.55f, 1f);
    public Color CoordinatorAccent = new(1f, 0.73f, 0.2f);
    public Color CrisisAccent = new(1f, 0.27f, 0.27f);
    public Color CriticalAlert = new(1f, 0.2f, 0.2f);
    public Color WarningAlert = new(1f, 0.62f, 0.18f);
}
