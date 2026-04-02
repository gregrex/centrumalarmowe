using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Session;

public sealed class OfflineOnlineSwitchController : MonoBehaviour
{
    [SerializeField] private bool isOnline;

    public bool IsOnline => isOnline;

    public void SetOnline(bool value)
    {
        isOnline = value;
    }
}
