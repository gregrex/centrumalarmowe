using Alarm112.Application.Interfaces;
using Alarm112.Application.Models;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class ShowcaseDemoService : IShowcaseDemoService
{
    private readonly IContentBundleLoader _loader;

    public ShowcaseDemoService(IContentBundleLoader loader) => _loader = loader;

    public async Task<ShowcaseMissionDto> GetShowcaseMissionAsync(CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<ShowcaseMissionJson>("showcase-mission.v1.json", cancellationToken);
        var steps = json.Beats.Select((beat, i) => new ShowcaseMissionStepDto(
            $"step.{beat}", beat, $"showcase.step.{beat}.desc", i + 1, true)).ToArray();
        return new ShowcaseMissionDto(
            json.MissionId, json.Title, json.RecommendedRole, json.EstimatedDurationSeconds, steps);
    }

    public async Task<OnboardingFlowDto> GetOnboardingFlowAsync(CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<OnboardingFlowJson>("onboarding-flow.v1.json", cancellationToken);
        var stepIds = json.Steps.Select(s => s.Id).ToArray();
        var hints = new[]
        {
            new OnboardingHintDto("hint.map.tap", "first_open_map", "Dotknij incydentu, aby zobaczyc szczegoly i ETA.", "info"),
            new OnboardingHintDto("hint.dispatch.reserve", "first_recovery", "Trzymaj przynajmniej jedna jednostke w rezerwie.", "warning"),
            new OnboardingHintDto("hint.report.retry", "post_partial_report", "Przy powtorce zareaguj wczesniej na wzrost presji.", "coach")
        };
        return new OnboardingFlowDto(json.FlowId, stepIds, hints);
    }

    public Task<IReadOnlyList<OnboardingHintDto>> GetHintSystemAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<OnboardingHintDto> payload = new[]
        {
            new OnboardingHintDto("hint.operator.summary", "mission_entry", "Operator filtruje informacje. Dispatcher zarzadza jednostkami.", "info"),
            new OnboardingHintDto("hint.capture.mode", "capture_mode_enabled", "Tryb capture wylacza zbedne overlaye.", "info"),
            new OnboardingHintDto("hint.retry.recommendation", "retry_preparation", "Zmien priorytet drugiego zgloszenia po 90 sekundach.", "coach")
        };
        return Task.FromResult(payload);
    }

    public Task<DemoPresentationFlowDto> GetDemoPresentationFlowAsync(CancellationToken cancellationToken)
    {
        var payload = new DemoPresentationFlowDto(
            "presentation.v20",
            new[] { "home", "onboarding", "chapter_map", "briefing", "runtime", "recovery_card", "scoreboard", "reward_reveal", "next_mission" },
            "9:16");
        return Task.FromResult(payload);
    }

    public Task<PlayerFacingPolishDto> GetPlayerFacingPolishAsync(CancellationToken cancellationToken)
    {
        var payload = new PlayerFacingPolishDto(
            "player-facing.v20",
            new[] { "animated_menu_layers", "coachmarks", "hint_strips", "reward_reveal_stingers", "capture_mode", "retry_next_polish" },
            "showcase-demo");
        return Task.FromResult(payload);
    }

    public Task<DemoCapturePlanDto> GetDemoCapturePlanAsync(CancellationToken cancellationToken)
    {
        var payload = new DemoCapturePlanDto(
            "capture.v20",
            new[] { "home", "roles", "briefing", "dispatch", "recovery", "scoreboard", "reward", "next" },
            new[] { "Disable debug widgets", "Use showcase seed", "Prefer dispatcher role", "Keep one bot fill visible on mission entry" },
            true);
        return Task.FromResult(payload);
    }
}
