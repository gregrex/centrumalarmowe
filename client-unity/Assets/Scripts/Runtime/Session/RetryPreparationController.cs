namespace Alarm112.Client.Runtime.Session;

public sealed class RetryPreparationController
{
    public bool IsReadyForRetry { get; private set; }

    public void PrepareRetry() => IsReadyForRetry = true;
}
