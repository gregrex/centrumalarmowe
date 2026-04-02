using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class TeamReadinessController : MonoBehaviour
{
    [SerializeField] private bool canStart;
    [SerializeField] private string botFillMode = "balanced_support";
    [SerializeField] private int teamScore = 78;

    public bool CanStart => canStart;
    public string BotFillMode => botFillMode;
    public int TeamScore => teamScore;

    public void SetReadyState(bool value)
    {
        canStart = value;
    }
}
