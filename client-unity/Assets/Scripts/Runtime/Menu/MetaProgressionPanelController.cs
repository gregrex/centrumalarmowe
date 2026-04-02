using UnityEngine;

namespace Alarm112.Client.Menu
{
    public sealed class MetaProgressionPanelController : MonoBehaviour
    {
        public void ShowReward(string rewardId)
        {
            Debug.Log($"[MetaProgressionPanelController] reward => {rewardId}");
        }
    }
}
