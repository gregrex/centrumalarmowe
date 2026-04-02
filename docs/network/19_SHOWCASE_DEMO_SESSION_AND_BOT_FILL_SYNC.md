# Showcase Demo Session and Bot Fill Sync

## Założenia
- demo build musi działać offline i online,
- brakujący gracze są uzupełniani botami po czasie timeout,
- ready state ma deterministic fallback,
- replayability nie może niszczyć nagrania showcase.

## Eventy sieciowe
- showcase.seed.loaded,
- onboarding.step.advanced,
- botfill.auto.assigned,
- capture.mode.enabled,
- showcase.mission.completed.
