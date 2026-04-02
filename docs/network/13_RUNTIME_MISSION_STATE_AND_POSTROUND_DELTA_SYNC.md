# RUNTIME MISSION STATE AND POSTROUND DELTA SYNC

## Co synchronizujemy
- active mission state
- objective deltas
- dispatch outcome events
- event feed items
- gate state
- postround report reveal step

## Minimalne zdarzenia
- mission.runtime.snapshot
- mission.objective.delta
- mission.dispatch.outcome
- mission.gate.opened
- mission.report.reveal

## Wymaganie dla bot fill
Bot musi reagować na:
- brak ACK,
- krytyczny objective,
- brak wolnej jednostki,
- gate blisko fail.
