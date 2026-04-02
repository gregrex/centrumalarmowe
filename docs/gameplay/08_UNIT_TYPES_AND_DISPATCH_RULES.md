# Unit types and dispatch rules

## Cel
Uporządkować jednostki i reguły wysyłki, żeby gameplay był logiczny i łatwy do balansowania.

## Typy jednostek MVP
- unit.ambulance.basic
- unit.ambulance.advanced
- unit.fire.engine
- unit.fire.ladder
- unit.police.patrol
- unit.police.intervention
- unit.tech.support
- unit.command.mobile

## Kluczowe atrybuty jednostki
- response domain,
- max severity handled,
- seat count,
- speed class,
- stabilization power,
- conflict resolution power,
- fire suppression power,
- technical utility,
- cooldown,
- availability.

## Reguły dispatch
- medyczne high/critical zawsze preferują najbliższą zgodną karetkę,
- pożar budynku wymaga co najmniej jednej jednostki fire.engine,
- agresja z ryzykiem przemocy wymaga policji przed tech support,
- zdarzenia masowe aktywują dispatch bundle,
- jeśli brak idealnej jednostki, system proponuje best available fallback.

## Quality gate
Każda nowa jednostka musi mieć:
- stable ID,
- kategorię,
- constraints,
- ikonę,
- słownik lokalizacji,
- test walidacyjny.
