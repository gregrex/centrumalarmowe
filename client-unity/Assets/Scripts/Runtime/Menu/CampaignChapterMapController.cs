using System.Collections.Generic;
using UnityEngine;

namespace Alarm112.ClientUnity.Runtime.Menu;

public sealed class CampaignChapterMapController : MonoBehaviour
{
    [SerializeField] private string defaultChapterId = "chapter.01";
    [SerializeField] private List<string> visibleNodeIds = new();

    public string CurrentChapterId { get; private set; } = string.Empty;

    private void Awake()
    {
        CurrentChapterId = defaultChapterId;
    }

    public void SetVisibleNodes(IEnumerable<string> nodeIds)
    {
        visibleNodeIds = new List<string>(nodeIds);
    }

    public void SelectChapter(string chapterId)
    {
        CurrentChapterId = chapterId;
    }
}
