# Active incident board i filtry

## Active incident board
Każda karta incydentu pokazuje:
- severity,
- typ incydentu,
- dzielnicę,
- licznik czasu,
- presję,
- wymagane role,
- status shared action,
- recommended unit.

## Filtry mapy
### Kategorie
- severity,
- służba,
- status jednostki,
- dzielnica,
- shared action required,
- bot assisted.

## Stany UI
- loading,
- empty,
- filtered-empty,
- stale data,
- offline fallback,
- warning route,
- critical overload.

## Edge cases
- brak jednostki spełniającej filtr,
- incydent już zamknięty przy próbie dispatchu,
- filtr ukrył wszystkie aktywne incydenty,
- współdzielona akcja wygasła zanim gracz potwierdził.
