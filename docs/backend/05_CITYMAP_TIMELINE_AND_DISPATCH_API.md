# City Map, Timeline and Dispatch API

## Endpoints for V6
### GET /api/city-map
Returns:
- city id,
- map preset,
- nodes,
- connections,
- district overlays,
- legend ids.

### GET /api/sessions/{sessionId}/timeline
Returns:
- ordered timeline items,
- severity,
- actor role,
- human or AI marker,
- short message,
- optional related incident id.

### POST /api/sessions/{sessionId}/dispatch
Accepts:
- incident id,
- unit id,
- requested action id,
- actor role,
- source = human|bot.

Returns:
- accepted,
- dispatch result code,
- ETA,
- pressure delta,
- warning code,
- timeline item inserted,
- updated hints.

## API goals
- easy to test,
- easy to stub in Unity,
- stable for smoke tests,
- dictionary-driven,
- no heavy infrastructure required.
