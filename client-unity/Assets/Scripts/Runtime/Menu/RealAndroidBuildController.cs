using UnityEngine;

namespace Alarm112.Client.Menu;

public sealed class RealAndroidBuildController : MonoBehaviour
{
    [SerializeField] private string buildStatus = "candidate_for_real_build";

    public void BindStatus(string value)
    {
        buildStatus = value;
        Debug.Log($"Real Android build status: {buildStatus}");
    }
}
