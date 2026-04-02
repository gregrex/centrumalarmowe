using UnityEngine;

namespace Alarm112.Client.Session;

public sealed class RuntimeDispatchOutcomeController : MonoBehaviour
{
    public void ShowOutcome(string outcome)
    {
        Debug.Log($"[RuntimeDispatchOutcomeController] Outcome: {outcome}");
    }
}
