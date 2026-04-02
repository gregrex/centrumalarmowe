namespace Alarm112.Client.Runtime.Session;

public sealed class MissionFailBranchController
{
    public string BranchId { get; private set; } = "fail.primary_objective";
    public string RetryHint { get; private set; } = "Prioritize EMS route stabilization earlier.";

    public void Bind(string branchId, string retryHint)
    {
        BranchId = branchId;
        RetryHint = retryHint;
    }
}
