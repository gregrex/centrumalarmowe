using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class ChapterRuntimeController : MonoBehaviour
{
    [SerializeField] private string activeChapterId = "chapter.01";
    [SerializeField] private string focusedMissionId = "mission.01.03";

    public string ActiveChapterId => activeChapterId;
    public string FocusedMissionId => focusedMissionId;

    public void FocusMission(string missionId)
    {
        focusedMissionId = missionId;
    }
}
