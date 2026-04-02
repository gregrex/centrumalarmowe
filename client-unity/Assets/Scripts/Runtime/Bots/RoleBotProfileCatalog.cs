using UnityEngine;

namespace Alarm112.Client.Runtime.Bots;

public sealed class RoleBotProfileCatalog : MonoBehaviour
{
    [SerializeField] private TextAsset? profilesJsonAsset;
    [SerializeField] private string fallbackPath = "data/content/role-bot-profiles.v2.json";
    [SerializeField] private int profileCount;

    public int ProfileCount => profileCount;

    public void LoadProfiles()
    {
        var json = Alarm112.Client.Runtime.Config.JsonConfigLoader.LoadTextAssetOrFile(profilesJsonAsset, fallbackPath);
        profileCount = string.IsNullOrWhiteSpace(json) ? 0 : Mathf.Max(1, json.Split("roleId").Length - 1);
        Debug.Log($"[RoleBotProfileCatalog] profiles={profileCount}");
    }
}
