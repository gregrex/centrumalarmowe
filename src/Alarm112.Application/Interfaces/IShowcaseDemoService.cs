using Alarm112.Contracts;

namespace Alarm112.Application.Interfaces;

public interface IShowcaseDemoService
{
    Task<ShowcaseMissionDto> GetShowcaseMissionAsync(CancellationToken cancellationToken);
    Task<OnboardingFlowDto> GetOnboardingFlowAsync(CancellationToken cancellationToken);
    Task<IReadOnlyList<OnboardingHintDto>> GetHintSystemAsync(CancellationToken cancellationToken);
    Task<DemoPresentationFlowDto> GetDemoPresentationFlowAsync(CancellationToken cancellationToken);
    Task<PlayerFacingPolishDto> GetPlayerFacingPolishAsync(CancellationToken cancellationToken);
    Task<DemoCapturePlanDto> GetDemoCapturePlanAsync(CancellationToken cancellationToken);
}
