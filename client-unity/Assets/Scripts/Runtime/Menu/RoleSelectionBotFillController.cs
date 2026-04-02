using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class RoleSelectionBotFillController : MonoBehaviour
{
    [SerializeField] private string selectedRole = "dispatcher";
    [SerializeField] private string botFillStyle = "balanced_support";
    [SerializeField] private bool autoFillMissingSlots = true;

    public string SelectedRole => selectedRole;
    public string BotFillStyle => botFillStyle;
    public bool AutoFillMissingSlots => autoFillMissingSlots;

    public void SelectRole(string roleId)
    {
        selectedRole = roleId;
    }

    public void SetBotFillStyle(string styleId)
    {
        botFillStyle = styleId;
    }

    public void SetAutoFill(bool value)
    {
        autoFillMissingSlots = value;
    }
}
