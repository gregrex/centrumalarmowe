# Session Deltas, Dispatch Ticks, Timeline Feed

## Why V6 needs this
The game now starts moving from static snapshots toward:
- dispatch actions,
- lightweight state deltas,
- timeline inserts,
- reconnect-safe feed rebuilds.

## Message families
- `session.snapshot.full`
- `session.delta.incident`
- `session.delta.unit`
- `session.delta.pressure`
- `session.timeline.insert`
- `session.dispatch.result`
- `session.role.handoff`
- `session.role.reclaim`

## V6 simplification
Do not implement deep event sourcing yet.
Use:
- one simple envelope,
- small payloads,
- session id,
- event type,
- timestamp,
- source role,
- compact payload.

## Reconnect strategy
- reconnecting client fetches full snapshot,
- then fetches timeline,
- then resumes hub delta feed.

## Timeline requirement
Every meaningful automated or human dispatch action must create a timeline row.
This becomes the bridge between:
- live gameplay,
- debugging,
- round report,
- QA verification.
