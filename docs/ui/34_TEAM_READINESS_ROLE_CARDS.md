# Team Readiness + Role Cards

## Cel
Przed startem rundy dać jasny obraz składu zespołu, gotowości i fallbacku botów.

## Sloty
- operator,
- dispatcher,
- coordinator,
- crisis_officer.

## Stan role card
- portrait,
- role name,
- player/bot/open/locked,
- readiness,
- network quality,
- fallback note,
- expected workload,
- difficulty modifier.

## Akcje
- reserve role,
- release role,
- set ready,
- change bot mode,
- preview bot profile,
- confirm start,
- cancel session.

## Reguły
- jeśli gracz offline: slot może być player_local albo bot_assigned,
- jeśli slot niegotowy: start zablokowany,
- jeśli host timeout: bot takeover preview aktywny,
- jeśli tryb solo: 3 sloty bot i 1 slot player.

## Audio/UI
- miękki confirm przy ready,
- mocny stinger przy all_ready,
- ostrzeżenie gdy brakuje krytycznej roli.
