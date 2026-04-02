namespace Alarm112.Client.Runtime.Session;

public sealed class ChainEscalationController
{
    public string ActiveChainId { get; private set; } = "chain.bridge.01";
    public string Severity { get; private set; } = "high";

    public void Bind(string chainId, string severity)
    {
        ActiveChainId = chainId;
        Severity = severity;
    }
}
