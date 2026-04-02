using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class PlayerIdentityPanelController : MonoBehaviour
{
    [SerializeField] private string displayName = "Dispatcher Zero";
    [SerializeField] private string preferredRole = "dispatcher";
    [SerializeField] private string portraitId = "portrait.dispatcher.steel";
    [SerializeField] private string frameId = "frame.blackout.pulse";
    [SerializeField] private string badgeId = "badge.fast_filter";

    public void ApplyIdentity(string playerName, string roleId, string portrait, string frame, string badge)
    {
        displayName = playerName;
        preferredRole = roleId;
        portraitId = portrait;
        frameId = frame;
        badgeId = badge;
    }
}
