# Kontrakty API

## Bootstrap
Zwraca:
- wersję klienta,
- aktywny bundle contentu,
- config UI,
- role,
- event categories,
- director presets,
- audio presets,
- bot presets.

## SessionAction
Przyjmuje:
- sessionId,
- actorId,
- role,
- actionType,
- payload,
- clientTime,
- correlationId.

## SessionState
Zwraca:
- summary,
- active incidents,
- available units,
- role panels,
- alerts,
- bot status,
- KPI.

## TelemetryEvent
Przyjmuje:
- eventName,
- playerId,
- role,
- screen,
- actionContext,
- success/failure,
- durationMs.
