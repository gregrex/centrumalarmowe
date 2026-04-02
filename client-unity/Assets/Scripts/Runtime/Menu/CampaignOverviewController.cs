using UnityEngine;

namespace Alarm112.Client.Menu
{
    public sealed class CampaignOverviewController : MonoBehaviour
    {
        [SerializeField] private string activeNodeId = "mission.01.03";

        public void BindNode(string nodeId)
        {
            activeNodeId = nodeId;
            Debug.Log($"[CampaignOverviewController] active node => {activeNodeId}");
        }
    }
}
