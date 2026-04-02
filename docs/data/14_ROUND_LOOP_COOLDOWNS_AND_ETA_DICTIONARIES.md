# Słowniki v8: round loop, cooldowny i ETA

## Nowe grupy
- round.phase,
- delta.type,
- unit.cooldown.reason,
- eta.band,
- overlay.style,
- heatmap.preset,
- split.panel.layout.

## Zasady
- każdy klucz ma stabilne ID,
- tekst lokalizacyjny jest osobno,
- JSON musi być prosty do importu i diffowania,
- brak liczb zaszytych w kodzie jeśli mogą być w słowniku lub configu.

## Przykłady
- `round.phase.alert-window`
- `delta.type.escalated`
- `delta.type.timeout-risk`
- `unit.cooldown.reason.refuel`
- `eta.band.fast`
- `overlay.style.warning`
- `split.panel.layout.operator-dispatcher`
