using UnityEngine;

namespace Alarm112.Client.Audio
{
    public sealed class SfxRouter : MonoBehaviour
    {
        public void PlayUiConfirm() => Debug.Log("[SfxRouter] ui.confirm");
        public void PlayUiWarning() => Debug.Log("[SfxRouter] ui.warning");
        public void PlayRoleRadio(string roleId) => Debug.Log($"[SfxRouter] radio => {roleId}");
    }
}
