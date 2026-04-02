using UnityEngine;

namespace Alarm112.Client.Runtime.Session;

public sealed class HalfPlayableRoundController : MonoBehaviour
{
    [SerializeField] private TextAsset? roundLoopJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/coop-round-loop.v1.json";
    [SerializeField] private int roleCount;
    [SerializeField] private int incidentCount;
    [SerializeField] private bool hasSharedAction;

    public int RoleCount => roleCount;
    public int IncidentCount => incidentCount;
    public bool HasSharedAction => hasSharedAction;

    public void LoadRound()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(roundLoopJsonAsset, fallbackPath);
        roleCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("role.").Length - 1);
        incidentCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(0, json.Split("inc.").Length - 1);
        hasSharedAction = json.Contains("shared.");
        Debug.Log($"[HalfPlayableRoundController] roles={roleCount}, incidents={incidentCount}, shared={hasSharedAction}");
    }
}
