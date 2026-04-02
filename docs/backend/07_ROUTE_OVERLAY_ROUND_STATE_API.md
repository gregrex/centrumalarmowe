# API v8: route overlay, round state i delty

## Endpointy
- `GET /api/sessions/{sessionId}/route-overlay?incidentId=...&unitId=...`
- `GET /api/sessions/{sessionId}/round-state`
- `GET /api/sessions/{sessionId}/live-deltas`
- `GET /api/sessions/{sessionId}/units/runtime`

## Minimalne kontrakty
### route overlay
- sessionId,
- incidentId,
- unitId,
- heatmapPreset,
- segments[].

### round state
- roundId,
- tick,
- elapsedSeconds,
- phase,
- activeHumanPlayers,
- activeBotPlayers,
- openIncidents,
- sharedActionsPending,
- rolePanels[].

### live deltas
- deltaId,
- incidentId,
- changeType,
- severity,
- status,
- timerDeltaSeconds,
- needsAttention,
- timestamp.

### units runtime
- unitId,
- callSign,
- unitType,
- status,
- currentNodeId,
- cooldownSeconds,
- etaSeconds,
- available,
- isBotBackfilled.

## Implementacja v8
Szkielet może zwracać dane hardcoded lub z JSON.
