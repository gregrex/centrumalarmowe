# Shared action locks i acknowledgements

## Założenie
Shared action nie może być przypadkowo wykonana dwa razy.

## Reguły sieciowe
- każda akcja ma `sharedActionId`,
- pierwszy request zakłada lock logiczny,
- ACK ma `role`, `accepted`, `isBot`,
- po quorum akcja emituje envelope `session.shared-action.resolved`.

## Bezpieczne uproszczenie MVP
- lock in-memory,
- brak rozproszonego locka,
- retry tylko idempotentne,
- brak optimistic merge dla dwóch sprzecznych shared actions na tym samym incydencie.
