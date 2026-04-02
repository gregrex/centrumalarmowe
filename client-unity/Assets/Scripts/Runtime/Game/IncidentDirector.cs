using UnityEngine;

namespace Alarm112.Client.Runtime.Game;

public sealed class IncidentDirector : MonoBehaviour
{
    [SerializeField] private float pressureScore;

    public float PressureScore => pressureScore;

    public void ApplyDelta(float delta)
    {
        pressureScore = Mathf.Clamp(pressureScore + delta, 0f, 100f);
    }
}
