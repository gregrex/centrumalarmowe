# City Map Dispatch Runtime

## Runtime loop
1. Incident appears on map.
2. Director assigns severity and timer.
3. Nearby units are filtered.
4. Player or bot selects a response.
5. Dispatch command is sent.
6. Session result returns ETA, pressure delta and warnings.
7. Timeline gets a new item.
8. Incident either stabilizes, escalates or remains active.
9. Round report aggregates timeline.

## Dispatch decision inputs
- incident category,
- incident severity,
- current elapsed time,
- available unit types,
- estimated route quality,
- blocked path flags,
- city pressure,
- missing role bot support,
- scenario-specific modifiers.

## Core outcomes
- correct dispatch,
- under-response,
- over-response,
- delayed dispatch,
- blocked dispatch,
- duplicate dispatch,
- bot auto-assist,
- escalation after dispatch.

## Gameplay goals
- reward correct prioritization,
- make overloaded decisions legible,
- allow recovery after mistakes,
- keep coop meaningful,
- keep solo with bots viable.

## V6 simplification
No deep simulation of roads yet.
Use:
- zone adjacency,
- placeholder ETA,
- route note text,
- simple pressure delta.

This keeps the build easy while still giving a real game loop.
