using UnityEngine;

namespace Alarm112.Client.Runtime.Bots;

public sealed class RoleBotDeltaResponder : MonoBehaviour
{
    [SerializeField] private TextAsset? deltasJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/live-incident-deltas.v1.json";
    [SerializeField] private int escalationCount;
    [SerializeField] private int sharedActionCount;

    public int EscalationCount => escalationCount;
    public int SharedActionCount => sharedActionCount;

    public void Evaluate()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(deltasJsonAsset, fallbackPath);
        escalationCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("delta.type.escalated").Length - 1);
        sharedActionCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("delta.type.shared-action-needed").Length - 1);
        Debug.Log($"[RoleBotDeltaResponder] escalation={escalationCount}, sharedAction={sharedActionCount}");
    }
}
