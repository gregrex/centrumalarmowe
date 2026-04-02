using Alarm112.Application.Interfaces;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class FinalHandoffService : IFinalHandoffService
{
    public Task<FinalHandoffPackDto> GetFinalHandoffPackAsync(CancellationToken cancellationToken)
    {
        var payload = new FinalHandoffPackDto(
            "v25",
            "android",
            "showcase.mission.01",
            new[] { "build", "store_compliance", "internal_demo", "playtest_loop", "liveops_feedback" },
            "ready_for_agent_handoff",
            "Run smoke-v25, review store compliance, then produce first Android test build.");
        return Task.FromResult(payload);
    }

    public Task<StoreComplianceDto> GetStoreComplianceAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<StoreComplianceItemDto> items = new[]
        {
            new StoreComplianceItemDto("sc25_01", "Privacy copy reviewed", "todo", "producer", "high"),
            new StoreComplianceItemDto("sc25_02", "Screenshots match gameplay", "in_progress", "ux", "high"),
            new StoreComplianceItemDto("sc25_03", "Internal testing notes prepared", "done", "qa", "medium")
        };
        var payload = new StoreComplianceDto("google_play_internal_testing", items, "Use one-mission showcase as the only public-facing internal build scope.");
        return Task.FromResult(payload);
    }

    public Task<InternalDemoPackageDto> GetInternalDemoPackageAsync(CancellationToken cancellationToken)
    {
        var payload = new InternalDemoPackageDto(
            "internal_demo_v25",
            new[] { "home", "onboarding", "briefing", "runtime", "report", "retry_or_next" },
            new[] { "stakeholders", "qa", "design", "engineering" },
            "Demo ma pokazać jedną misję showcase bez przeładowywania funkcjami pobocznymi.");
        return Task.FromResult(payload);
    }

    public Task<PlaytestReleaseLiveopsLoopDto> GetPlaytestReleaseLiveopsLoopAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<PlaytestLoopStageDto> stages = new[]
        {
            new PlaytestLoopStageDto("pl25_01", "Playtest", "qa_lead", "active", "feedback_form"),
            new PlaytestLoopStageDto("pl25_02", "Feedback triage", "producer", "queued", "severity_board"),
            new PlaytestLoopStageDto("pl25_03", "LiveOps review", "analyst", "queued", "liveops_panel"),
            new PlaytestLoopStageDto("pl25_04", "Patch scope", "tech_lead", "queued", "next_build_plan")
        };
        var payload = new PlaytestReleaseLiveopsLoopDto(stages, "playtest", "Collect first focused internal feedback on showcase mission.");
        return Task.FromResult(payload);
    }

    public Task<ReleaseReadinessV25Dto> GetReleaseReadinessV25Async(CancellationToken cancellationToken)
    {
        IReadOnlyList<ReleaseReadinessChecklistItemDto> checklist = new[]
        {
            new ReleaseReadinessChecklistItemDto("rr25_01", "Smoke-v25 passed", "done", "P1"),
            new ReleaseReadinessChecklistItemDto("rr25_02", "Final handoff pack ready", "done", "P1"),
            new ReleaseReadinessChecklistItemDto("rr25_03", "Store compliance review in progress", "in_progress", "P1")
        };
        var payload = new ReleaseReadinessV25Dto(checklist, "handoff_candidate", "Generate Android internal build and begin focused playtest.");
        return Task.FromResult(payload);
    }
}
