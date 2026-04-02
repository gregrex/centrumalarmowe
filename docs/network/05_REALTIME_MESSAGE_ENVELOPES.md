# Realtime message envelopes

## Cel
Ujednolicić komunikację klient-serwer i uprościć debugowanie. Wszystkie wiadomości realtime mają ten sam wrapper.

## Envelope
- messageId
- sessionId
- messageType
- issuedAtUtc
- senderRole
- correlationId
- schemaVersion
- payload

## Przykładowe typy wiadomości
- lobby.state.changed
- lobby.slot.updated
- session.snapshot.full
- session.snapshot.delta
- incident.created
- incident.updated
- unit.status.changed
- alert.raised
- bot.takeover.started
- bot.takeover.ended
- player.reconnected
- readycheck.started
- scenario.loading
- scenario.started
- scenario.finished

## Zasady
- żadnych gołych anonimowych payloadów bez typu,
- wszystkie typy wersjonowane,
- payload ma być mały i czytelny,
- snapshot full tylko gdy naprawdę potrzeba,
- preferuj delty z czytelnym `entityId`.

## Quality gate
Jeśli agent dodaje nowy typ wiadomości, musi też:
- dopisać go do słownika typów,
- opisać w dokumentacji,
- dodać przykład payloadu,
- uwzględnić logowanie i trace.
