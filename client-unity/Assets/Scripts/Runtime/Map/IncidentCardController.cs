using UnityEngine;

namespace Alarm112.Client.Runtime.Map;

public sealed class IncidentCardController : MonoBehaviour
{
    [SerializeField] private string selectedIncidentId = "incident.demo.001";
    [SerializeField] private string suggestedActionId = "action.dispatch.ambulance";
    [SerializeField] private string suggestedUnitId = "AMB-01";

    public void BindIncident(string incidentId, string actionId, string unitId)
    {
        selectedIncidentId = incidentId;
        suggestedActionId = actionId;
        suggestedUnitId = unitId;
        Debug.Log($"[IncidentCard] incident={selectedIncidentId} action={suggestedActionId} unit={suggestedUnitId}");
    }
}
