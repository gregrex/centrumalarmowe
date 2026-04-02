# V6 Scope — City Map, Live Dispatch, Timeline, Mobile Readability

## Goal
Deliver the next **easy-to-build but clearly playable** vertical slice:
- 2D city map with readable zones and markers,
- live unit roster,
- incident card with dispatch actions,
- basic dispatch request sent to API,
- session timeline and end report timeline,
- bot fallback when human role is missing,
- premium but production-friendly 2D visuals.

## Why this scope
V5 proved the flow:
Home -> Quick Play -> Lobby -> Session -> Bot fallback -> Report.

V6 must prove the **moment-to-moment loop**:
1. player sees an incident,
2. player reads map and available units,
3. player dispatches the best unit,
4. session reacts,
5. timeline records the result,
6. round report explains the consequence.

## Quality constraints
- mobile-first,
- 2D only,
- premium readability over complexity,
- SVG/placeholder friendly,
- dictionary driven data,
- minimal hardcoded content,
- single-command local smoke.

## Mandatory systems
- city graph and zone ids,
- dispatchable unit roster,
- incident action catalog,
- timeline items,
- dispatch API and simple result model,
- bot assist for missing roles,
- per-role audio and HUD priority guidance.

## DoD for V6
- `/api/city-map` returns a valid city map payload,
- `/api/sessions/{id}/timeline` returns timeline data,
- `/api/sessions/{id}/dispatch` accepts a dispatch command and returns a result,
- Unity starter contains controllers for map, live units, incident card and timeline,
- content validation includes city-map and incident-actions packs,
- smoke script validates V6 critical files,
- docs explain build simplicity and art quality rules.

## Out of scope
- final production netcode,
- full map rendering,
- real pathfinding,
- final art export pipeline,
- complete economy,
- deep progression.
