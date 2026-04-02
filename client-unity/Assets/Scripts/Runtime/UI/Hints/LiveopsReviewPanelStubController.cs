namespace Alarm112.ClientUnity.Runtime.UI.Hints;

public sealed class LiveopsReviewPanelStubController
{
    public string ReviewState { get; private set; } = "draft_demo";

    public void SetState(string reviewState)
    {
        ReviewState = reviewState;
    }
}
