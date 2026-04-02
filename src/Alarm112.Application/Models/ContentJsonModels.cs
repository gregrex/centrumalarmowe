namespace Alarm112.Application.Models;

// --- Home ---
internal sealed record HomeHubJson(
    string Version,
    string DefaultScreen,
    HomeHubContinueJson? ContinueSession,
    HomeCardJson[] Cards);

internal sealed record HomeHubContinueJson(string SessionId, string LabelKey, string Summary);
internal sealed record HomeCardJson(string Id, string Type, string LabelKey, string State, string Route);

// --- Campaign ---
internal sealed record CampaignChaptersJson(string Version, CampaignChapterJson[] Chapters);
internal sealed record CampaignChapterJson(string Id, string TitleKey, string ThemeId, double Progress, string[] NodeIds);

// --- Mission Entry ---
internal sealed record MissionEntryJson(
    string Version,
    string MissionId,
    string TitleKey,
    string ChapterId,
    int EstimatedMinutes,
    string Difficulty,
    string RecommendedRole,
    string WeatherPreset,
    string TimeOfDay,
    string[] StartingUnits,
    string[] RiskTags,
    string[] Rewards,
    int AvailableSlots,
    MissionEntryBotFillJson? BotFillSummary);

internal sealed record MissionEntryBotFillJson(bool Supported, string[] Modes, string DefaultMode);

// --- Mission Briefing ---
internal sealed record MissionBriefingJson(
    string MissionId,
    string TitleKey,
    string Difficulty,
    int EstimatedMinutes,
    string WeatherPreset,
    string TimeOfDay,
    string[] PrimaryObjectives,
    string[] SecondaryObjectives,
    string[] RiskTags,
    string[] RecommendedRoles,
    string[] SuggestedUnits,
    MissionSpeakerJson? Speaker,
    string[] Hotspots);

internal sealed record MissionSpeakerJson(string PortraitId, string LineKey);

// --- Team Readiness ---
internal sealed record TeamReadinessJson(
    string MissionId,
    string Mode,
    TeamReadinessSlotJson[] Slots,
    string BotFillMode,
    bool CanStart,
    string[] Warnings,
    int TeamScore);

internal sealed record TeamReadinessSlotJson(
    string RoleId,
    string State,
    string? OccupantId,
    bool Ready,
    string NetworkQuality);

// --- Post Round Report ---
internal sealed record PostRoundReportJson(
    string MissionId,
    string GradeId,
    int Score,
    int Stars,
    PostRoundObjectiveJson[] Objectives,
    PostRoundMetricJson[] Metrics,
    string[] Mistakes,
    string[] Rewards,
    string[] NextActions);

internal sealed record PostRoundObjectiveJson(string ObjectiveId, string State);
internal sealed record PostRoundMetricJson(string MetricId, int Value);

// --- Mission Complete Flow ---
internal sealed record MissionCompleteFlowJson(string[] Steps, string AudioState, string ScenePreset);

// --- Chapter Runtime ---
internal sealed record ChapterRuntimeJson(
    string ActiveChapterId,
    string CameraFocusNodeId,
    ChapterRuntimeChapterJson[] Chapters,
    ChapterRuntimeNodeJson[] NodeStates);

internal sealed record ChapterRuntimeChapterJson(string ChapterId, string TitleKey, double Completion, string WeatherPreset);
internal sealed record ChapterRuntimeNodeJson(string MissionId, string State, int Stars);

// --- Role Selection Bot Fill ---
internal sealed record RoleSelectionBotFillJson(
    string MissionId,
    string RecommendedRole,
    RoleSelectionSlotJson[] Slots,
    BotFillPreviewJson? BotPreview);

internal sealed record RoleSelectionSlotJson(string RoleId, string State, string? OccupantId, string Difficulty);
internal sealed record BotFillPreviewJson(string Style, string[] ExpectedWeaknesses, string SummaryKey);

// --- Round Bootstrap ---
internal sealed record RoundBootstrapJson(
    string MissionId,
    string Mode,
    string SelectedRole,
    string ScenePreset,
    string WeatherPreset,
    string MusicState,
    string[] StartingUnits,
    string[] RiskTags,
    RoundTeamSlotJson[] TeamSlots,
    string[] Objectives);

internal sealed record RoundTeamSlotJson(string RoleId, string State);

// --- Showcase Mission ---
internal sealed record ShowcaseMissionJson(
    string MissionId,
    string Title,
    string RecommendedRole,
    string[] Beats,
    int EstimatedDurationSeconds);

// --- Onboarding Flow ---
internal sealed record OnboardingFlowJson(string FlowId, OnboardingStepJson[] Steps);
internal sealed record OnboardingStepJson(string Id, string Title);

// --- Full Mission Runtime ---
internal sealed record FullMissionRuntimeJson(
    string MissionId,
    string TitleKey,
    string Difficulty,
    string RuntimeState,
    int CityStability,
    string PressureState,
    FullMissionIncidentJson[] ActiveIncidents,
    FullMissionUnitJson[] AvailableUnits);

internal sealed record FullMissionIncidentJson(
    string Id,
    string Type,
    string Priority,
    string Node,
    string AssignedUnitId);

internal sealed record FullMissionUnitJson(
    string Id,
    string Type,
    string Status,
    int EtaSeconds,
    int CooldownSeconds);

// --- Mission Script ---
internal sealed record MissionScriptJson(string MissionId, MissionScriptTimelineItemJson[] Timeline);
internal sealed record MissionScriptTimelineItemJson(int AtSeconds, string Event);
