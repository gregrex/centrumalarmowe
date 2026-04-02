# Live incident deltas i round ticks

## Dlaczego delty
Pełne snapshoty są cięższe i mniej czytelne. V8 wprowadza lekkie komunikaty:
- incident escalated,
- incident timeout risk,
- incident resolved,
- unit eta changed,
- shared action pending.

## Envelope typy v8
- `session.incident.delta`
- `session.round.tick`
- `session.units.runtime.delta`
- `session.route.overlay.updated`

## Tick
- round tick co 2 sekundy w szkielecie,
- w pełnej wersji zależny od scenariusza i trybu.

## Kolejność
1. gracz wykonuje akcję,
2. backend odpowiada,
3. hub publikuje deltę,
4. klient aktualizuje lokalny panel,
5. bot może zareagować po krótkim oknie.

## Reconnect
Po reconnect klient powinien dostać:
- last round state,
- active deltas z krótkiego bufora,
- unit runtime snapshot.
