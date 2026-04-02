
using UnityEngine;

namespace Alarm112.Client.Runtime.Session;

public sealed class PlayableSessionBootstrap : MonoBehaviour
{
    [SerializeField] private bool started;

    public bool Started => started;

    public void StartSession()
    {
        started = true;
        Debug.Log("[PlayableSession] vertical slice session started");
    }
}
