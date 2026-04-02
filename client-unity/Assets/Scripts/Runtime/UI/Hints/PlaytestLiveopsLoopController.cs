using UnityEngine;

namespace Alarm112.Client.UI.Hints;

public sealed class PlaytestLiveopsLoopController : MonoBehaviour
{
    public void ShowLoopState(string state)
    {
        Debug.Log($"Playtest/liveops loop state: {state}");
    }
}
