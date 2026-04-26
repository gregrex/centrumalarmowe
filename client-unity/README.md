# Unity Client Scaffold

Ten katalog zawiera pierwszy szkielet klienta 2D pod pionowe UI telefonu.
Docelowy engine: Unity 2D.


## V5 focus
- Home -> Quick Play -> Lobby -> Session -> End Report
- role HUD polish
- quick AI bot fallback

## Current scaffold
- `Networking/Alarm112ApiClient.cs` wraps the main demo/session HTTP calls.
- `Networking/SessionHubClient.cs` keeps local connection/session state for envelope + heartbeat handling during Unity-side integration.
- `UI/Huds/*HudController.cs` now parse session snapshot JSON into lightweight HUD state instead of staying as empty stubs.
- Quick Play scaffold uses canonical backend role names (`CallOperator`, `Dispatcher`, `OperationsCoordinator`, `CrisisOfficer`).
- Local lobby/API defaults point to the current dev API base: `http://localhost:5080`.
