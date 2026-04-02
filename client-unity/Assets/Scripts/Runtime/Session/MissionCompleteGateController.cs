using UnityEngine;

namespace Alarm112.Client.Session;

public sealed class MissionCompleteGateController : MonoBehaviour
{
    public void OpenGate(string gateState)
    {
        Debug.Log($"[MissionCompleteGateController] Gate: {gateState}");
    }
}
