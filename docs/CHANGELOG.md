# CHANGELOG.md — Historia zmian

> Format: [Keep a Changelog](https://keepachangelog.com/en/1.0.0/)

---

## [v26] — 2026-03-29 — Real Android Build + Bugfix Freeze

### Dodano
- `RealAndroidBuildService` z endpointem `/api/real-android-build`
- `OperatorDispatcherShowcaseDto` — showcase przepływu operator/dispatcher
- Dokumentacja `48_V26_REAL_ANDROID_BUILD_AND_BUGFIX_FREEZE_SCOPE.md`
- E2E Playwright tests project (`tests/e2e/`)
- `DEMO_SCRIPT.md`, `DEMO_REPORT.md`, `CHANGELOG.md`
- `DEPLOY.md`, `ADMIN_GUIDE.md`, `USER_GUIDE.md`
- Artefakty katalogów (`artifacts/e2e/`, `artifacts/demo/`)

### Naprawiono
- Health endpoint pokazuje v26 (był v25)
- `verify.ps1` — pełny skrypt build+test+smoke (był placeholder)
- `docker-verify.ps1` — smoke po `docker compose up`
- Dockerfile kopiuje `data/` do `/app/data`
- Healthchecks i named volumes w docker-compose

### Architektura
- CORS, rate limiting i error handling (ProblemDetails) aktywowane w `Program.cs`
- `find-free-port.ps1` i `find-free-port.sh` dodane do `tools/`

---

## [v25] — 2026-03-15 — Final Handoff Android Build

### Dodano
- `FinalHandoffService` z `FinalHandoffPackDto`
- `ReleaseReadinessV25Dto`, `ReleaseNotesDraftDto`
- Dokumentacja `46_V25_FINAL_HANDOFF_ANDROID_BUILD_SCOPE.md`

---

## [v24] — 2026-03-01 — Internal Test + Liveops

### Dodano
- `InternalTestService` z `InternalTestPackDto`, `InternalTestBuildStepDto`
- `LiveopsReviewPanelDto`, `LiveopsReviewWidgetDto`
- `PlaytestReleaseLiveopsLoopDto`, `PlaytestFeedbackFormDto`
- Dokumentacja `44_V24_INTERNAL_TEST_AND_LIVEOPS_SCOPE.md`

---

## [v23] — 2026-02-15 — Android Preview + Release Readiness

### Dodano
- `AndroidPreviewService`, `AndroidRcPipelineDto`, `GooglePlayInternalTestingDto`
- `StoreComplianceDto`, `StoreShotMockDto`
- Dokumentacja `42_V23_ANDROID_PREVIEW_AND_RELEASE_READINESS_SCOPE.md`

---

## [v22] — 2026-02-01 — Release Candidate Showcase

### Dodano
- `ReleaseCandidateService`, `ReleaseCandidatePackageDto`
- `BugfixFreezeDto`, `BugBashChecklistItemDto`
- `ReviewFeedbackDashboardDto`, `ReviewFeedbackSummaryDto`
- Dokumentacja `40_V22_RELEASE_CANDIDATE_SHOWCASE_SCOPE.md`

---

## [v21] — 2026-01-15 — Quasi-Final Showcase Build

### Dodano
- `ReviewBuildService`, `ReviewBuildPackageDto`
- `FinalPromoMediaDto`, `FinalTrailerStoreDemoDto`
- Dokumentacja `38_V21_QUASI_FINAL_SHOWCASE_BUILD_SCOPE.md`

---

## [v20] — 2026-01-01 — Showcase Demo Package

### Dodano
- `ShowcaseDemoService`, `ShowcaseMissionDto`, `ShowcaseMissionStepDto`
- `OnboardingFlowDto`, `OnboardingHintDto`
- `DemoPresentationFlowDto`, `DemoCapturePlanDto`
- Dokumentacja `36_V20_SHOWCASE_DEMO_PACKAGE_SCOPE.md`

---

## [v19] — 2025-12-15 — Scoreboard + Rewards + Retry Prep

### Dodano
- `NearFinalSliceService`, `NearFinalSliceFlowDto`
- `MetaProgressionDto`, `ProfileCosmeticDto`
- `RetryPreparationDto`, `FailRetryNextFlowDto`
- Dokumentacja `34_V19_SCOREBOARD_REWARDS_RETRYPREP_SCOPE.md`

---

## [v18] — 2025-12-01 — Runtime UI Recovery

### Dodano
- `RuntimeUiFlowService`, `RecoveryHudTriggerDto`, `RecoveryDecisionCardDto`
- `ReportRoomPolishDto`, `ReportRoomVariantDto`
- Dokumentacja `32_V18_RUNTIME_UI_RECOVERY_SCOPE.md`

---

## [v17] — 2025-11-15 — Quasi-Production Demo

### Dodano
- `QuasiProductionDemoService`, `QuasiProductionDemoFlowDto`
- `RuntimeScoreboardDto`, `PostRoundReportDto`
- `MissionCompleteFlowDto`, `SessionReportDto`
- Dokumentacja `30_V17_QUASI_PRODUCTION_DEMO_SCOPE.md`

---

## [v16] — 2025-11-01 — Live Route + Objective Timer

### Dodano
- `RuntimePolishService`, `LiveRouteRuntimeDto`, `LiveRouteSegmentStateDto`
- `ObjectiveTrackerDto`, `ObjectiveStateTransitionDto`
- Dokumentacja `28_V16_LIVE_ROUTE_OBJECTIVE_TIMER_SCOPE.md`

---

## [v15] — 2025-10-15 — Playable Runtime Map

### Dodano
- `PlayableRuntimeService`, `PlayableRuntimeMapDto`
- `DispatcherLoopDto`, `RuntimeDispatchOutcomeDto`
- `VisualRuntimeRouteLayerDto`
- Dokumentacja `26_V15_PLAYABLE_RUNTIME_MAP_SCOPE.md`

---

## [Vertical Slice] — 2025-06-01 — Pierwsze grywalne

### Dodano
- `SessionService`, `BotDirector`, `BotTickHostedService`
- `InMemorySessionStore`
- `DemoFactory`, `VerticalSliceFactory`
- `SessionHub` (SignalR)
- Podstawowe endpointy: `/health`, `/api/sessions/demo`, `/api/reference-data`
- Domain enums: `RoleType`, `SessionState`, `IncidentCategory`, `IncidentSeverity`
