using Alarm112.Application.Interfaces;
using Alarm112.Application.Models;
using Alarm112.Contracts;

namespace Alarm112.Application.Services;

public sealed class RuntimeBootstrapService : IRuntimeBootstrapService
{
    private readonly IContentBundleLoader _loader;

    public RuntimeBootstrapService(IContentBundleLoader loader) => _loader = loader;

    public async Task<IReadOnlyList<CampaignNodeRuntimeDto>> GetChapterRuntimeAsync(CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<ChapterRuntimeJson>("chapter-runtime.v1.json", cancellationToken);
        IReadOnlyList<CampaignNodeRuntimeDto> result = json.NodeStates
            .Select(n => new CampaignNodeRuntimeDto(n.MissionId, n.State, n.Stars, n.State == "active"))
            .ToArray();
        return result;
    }

    public async Task<RoleSelectionPreviewDto> GetRoleSelectionPreviewAsync(CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<RoleSelectionBotFillJson>("role-selection-botfill.v1.json", cancellationToken);
        var slots = json.Slots.Select(s => new RoleSelectionSlotDto(
            s.RoleId, s.State, s.OccupantId, s.Difficulty,
            s.State == "player_assigned",
            false)).ToArray();
        var botPreview = json.BotPreview is { } bp
            ? new BotFillPreviewDto(bp.Style, bp.ExpectedWeaknesses, bp.SummaryKey)
            : new BotFillPreviewDto("balanced_support", Array.Empty<string>(), "bot.preview.balanced_support");
        return new RoleSelectionPreviewDto(json.MissionId, json.RecommendedRole, slots, botPreview);
    }

    public async Task<RoundBootstrapDto> GetRoundBootstrapAsync(string? missionId, string? mode, string? role, CancellationToken cancellationToken)
    {
        var json = await _loader.LoadContentAsync<RoundBootstrapJson>("round-bootstrap.v1.json", cancellationToken);
        var resolved = string.IsNullOrWhiteSpace(missionId) ? json.MissionId : missionId;
        var resolvedMode = string.IsNullOrWhiteSpace(mode) ? json.Mode : mode;
        var resolvedRole = string.IsNullOrWhiteSpace(role) ? json.SelectedRole : role;

        var slots = json.TeamSlots.Select(s => new RoleSelectionSlotDto(
            s.RoleId, s.State, null, "medium",
            s.State == "player_assigned" && s.RoleId == resolvedRole,
            false)).ToArray();

        return new RoundBootstrapDto(
            resolved, resolvedMode, resolvedRole,
            json.ScenePreset, json.WeatherPreset, json.MusicState,
            json.StartingUnits, json.RiskTags, slots, json.Objectives);
    }

    public Task<IReadOnlyList<ObjectiveGradeDto>> GetObjectiveGradesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<ObjectiveGradeDto> payload = new[]
        {
            new ObjectiveGradeDto("A", 90, 3, new[] { "xp.300", "badge.perfect_start" }),
            new ObjectiveGradeDto("B", 75, 2, new[] { "xp.180" }),
            new ObjectiveGradeDto("C", 60, 1, new[] { "xp.100" }),
            new ObjectiveGradeDto("D", 45, 0, Array.Empty<string>()),
            new ObjectiveGradeDto("F", 0, 0, Array.Empty<string>())
        };
        return Task.FromResult(payload);
    }

    public Task<IReadOnlyList<VerticalSliceScenePackDto>> GetFinalVerticalSliceScenesAsync(CancellationToken cancellationToken)
    {
        IReadOnlyList<VerticalSliceScenePackDto> payload = new[]
        {
            new VerticalSliceScenePackDto("dispatch_hall_storm", "storm_layers_v2", new[] { "dispatch_console_v1", "chapter_map_board_v1" }, "chapter_map_focus"),
            new VerticalSliceScenePackDto("chapter_map_room", "chapter_room_layers_v1", new[] { "chapter_map_board_v1" }, "menu_home_base"),
            new VerticalSliceScenePackDto("operations_map_neon", "ops_neon_layers_v1", new[] { "radio_console_prop_v1" }, "mission_entry_tension")
        };
        return Task.FromResult(payload);
    }
}
