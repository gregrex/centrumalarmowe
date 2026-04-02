# Architektura sieciowa

## Model
Autoritarny backend sesji + klient mobilny.
Real-time synchronizacja przez SignalR lub WebSocket gateway.

## Moduły
- Session Service,
- Matchmaking Service,
- Presence Service,
- Reconnect Service,
- Bot Orchestrator,
- Event Director Service,
- Telemetry Ingest,
- Content/Config Service.

## Wymagania
- sesja 1–4 graczy,
- niski koszt wejścia,
- deterministyczne zdarzenia krytyczne,
- kolejka eventów,
- idempotencja komend,
- reconnect,
- bot takeover,
- late join albo controlled rejoin.

## Synchronizowane elementy
- role,
- aktywne zdarzenia,
- jednostki i ich status,
- alerty,
- KPI,
- director state,
- decyzje bota i graczy.
