
using UnityEngine;

namespace Alarm112.Client.Runtime.QuickPlay;

public sealed class QuickPlayBootstrapController : MonoBehaviour
{
    [SerializeField] private string scenarioId = "scenario.verticalslice.quickplay";
    [SerializeField] private string preferredRole = "CallOperator";
    [SerializeField] private bool autoFillBots = true;

    public string ScenarioId => scenarioId;
    public string PreferredRole => preferredRole;
    public bool AutoFillBots => autoFillBots;
}
