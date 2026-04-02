# Runtime Mission HUD V2

## Cel ekranu
HUD jednej demo-misji ma dać graczowi wszystko, co potrzebne do:
- zrozumienia stanu miasta,
- obsługi dispatch loop,
- reagowania na recovery cards,
- podjęcia decyzji retry / next po zakończeniu.

## Strefy HUD
1. Top bar:
   - mission timer,
   - mission title,
   - city pressure,
   - connectivity icon.
2. Left rail:
   - active incidents,
   - route status,
   - unit availability.
3. Center map layer:
   - live routes,
   - incidents,
   - blocked segments,
   - unit movement hints.
4. Right rail:
   - objective tracker,
   - recovery card slot,
   - event feed.
5. Bottom action bar:
   - assign,
   - reinforce,
   - reroute,
   - hold,
   - cancel,
   - confirm.

## Reguły czytelności
- czerwony tylko dla krytycznych stanów,
- amber dla ryzyka i recoveries,
- zielony dla stabilizacji i objective completion,
- maksymalnie 3 aktywne krytyczne banery naraz,
- recovery card nie może przykrywać objective tracker.

## Edge cases
- brak gracza w roli -> bot hint ribbon,
- brak jednostek -> CTA do reroute/reinforce,
- timer threshold -> audio + visual emphasis,
- fail branch -> dedykowany overlay.
