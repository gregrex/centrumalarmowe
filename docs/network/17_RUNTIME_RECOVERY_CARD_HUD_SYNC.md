# Runtime Recovery Card HUD Sync

## Synchronizowane elementy
- active recovery card id,
- card severity,
- selected option,
- decision timestamp,
- bot recommendation,
- fail/retry/next state.

## Wymagania
- host/source of truth w API,
- klient renderuje tylko stan,
- recovery card ack musi być broadcastowane,
- rejoin odtwarza ostatni aktywny card state.
