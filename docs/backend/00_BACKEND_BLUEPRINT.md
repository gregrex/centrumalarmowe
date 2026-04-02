# Backend blueprint

## Moduły backendu
- Profiles,
- Sessions,
- Matchmaking,
- Scenarios,
- Event Catalog,
- AI Bots,
- Director Config,
- Telemetry,
- LiveOps,
- Admin CMS.

## Główne endpointy
- /health
- /api/bootstrap
- /api/profile
- /api/scenarios
- /api/session/create
- /api/session/join
- /api/session/reconnect
- /api/session/action
- /api/session/state
- /api/bots/config
- /api/liveops/bundles
- /api/telemetry/events

## Minimalny model danych
- player_profile,
- session,
- session_participant,
- session_role_state,
- incident_template,
- unit_template,
- city_zone,
- director_rule,
- bot_profile,
- bot_action_log,
- telemetry_event,
- content_bundle.
