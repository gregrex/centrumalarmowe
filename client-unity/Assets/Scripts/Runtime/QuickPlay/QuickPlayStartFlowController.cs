using UnityEngine;

namespace Alarm112.Client.Runtime.QuickPlay;

public sealed class QuickPlayStartFlowController : MonoBehaviour
{
    [SerializeField] private bool requested;
    [SerializeField] private string lastSessionId = string.Empty;

    public bool Requested => requested;
    public string LastSessionId => lastSessionId;

    public void MarkStarted(string sessionId)
    {
        requested = true;
        lastSessionId = sessionId;
        Debug.Log($"[QuickPlayStartFlow] session started -> {sessionId}");
    }
}
