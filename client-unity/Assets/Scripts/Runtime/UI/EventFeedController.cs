using UnityEngine;

namespace Alarm112.Client.UI;

public sealed class EventFeedController : MonoBehaviour
{
    public void PushEvent(string labelKey)
    {
        Debug.Log($"[EventFeedController] Event: {labelKey}");
    }
}
