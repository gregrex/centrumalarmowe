using UnityEngine;

namespace Alarm112.Client.Menu;

public sealed class FinalHandoffPackageController : MonoBehaviour
{
    [SerializeField] private string statusLabel = "handoff_candidate";

    public void BindDemoStatus(string value)
    {
        statusLabel = value;
        Debug.Log($"Final handoff status: {statusLabel}");
    }
}
