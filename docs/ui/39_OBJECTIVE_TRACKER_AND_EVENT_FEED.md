# OBJECTIVE TRACKER AND EVENT FEED

## Objective tracker
Każdy objective powinien mieć:
- id,
- label key,
- status,
- progress value,
- priority,
- fail condition note.

Statusy:
- locked
- active
- completed
- failed

## Event feed
Feed ma być krótki, szybki i warstwowy.

Typy zdarzeń:
- dispatch issued
- dispatch outcome success
- dispatch outcome delayed
- route blocked
- objective advanced
- objective failed
- team warning
- mission complete gate

## Reguły UX
- nie pokazywać więcej niż 8 najświeższych wpisów
- wpisy krytyczne blokują się na 2 sekundy
- wpisy niskiego priorytetu łączą się w grupy
- klik w wpis przenosi fokus na incydent lub objective
