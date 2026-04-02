namespace Alarm112.ClientUnity.Runtime.Session;

public sealed class TestBuildDeployController
{
    public string EnvironmentName { get; private set; } = "review";
    public bool ReadyForInternalReview { get; private set; } = true;

    public void MarkNotReady() => ReadyForInternalReview = false;
    public void MarkReady() => ReadyForInternalReview = true;
}
