namespace Alarm112.ClientUnity.Runtime.Art;

public sealed class FinalTrailerStoreDemoController
{
    public string PresentationState { get; private set; } = "draft";

    public void Apply(string presentationState)
    {
        PresentationState = presentationState;
    }
}
