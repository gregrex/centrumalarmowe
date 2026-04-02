# SignalR Event Catalog

## Hub
`/hubs/session`

## Serwer -> klient
- `session.snapshot.full`
- `session.snapshot.delta`
- `session.alert.raised`
- `session.role.changed`
- `session.bot.takeover.started`
- `session.bot.takeover.stopped`
- `session.incident.created`
- `session.incident.updated`
- `session.incident.closed`
- `session.unit.updated`
- `session.phase.changed`
- `session.kpi.updated`
- `session.error`

## Klient -> serwer
- `session.join`
- `session.leave`
- `session.heartbeat`
- `player.assign.role`
- `player.submit.call-triage`
- `player.dispatch.unit`
- `player.reprioritize.incident`
- `player.request.backup`
- `player.resolve.alert`
- `player.pause.demo`

## Reguły
- snapshot full po wejściu i reconnect,
- delta co tick lub po zmianie stanu,
- heartbeat co 5–10 sekund,
- po braku heartbeat uruchamia się AI takeover,
- po powrocie gracza AI oddaje slot dopiero po bezpiecznym oknie synchronizacji.
