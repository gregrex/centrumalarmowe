# V15 — Playable Runtime Map Scope

## Cel
Przesunąć projekt z poziomu scaffold full-mission runtime do poziomu pierwszego naprawdę grywalnego loopa misji:
Home -> Briefing -> Ready Check -> Runtime Map -> Dispatcher Loop -> Objective State Changes -> Mission Complete -> Report -> Retry/Continue.

## Zakres v15
1. Playable runtime map z widocznymi strefami miasta, ikonami incydentów, trasami i live unit state.
2. Dispatcher loop z prostymi akcjami: assign, reroute, reinforce, hold, cancel.
3. Objective state machine: locked -> active -> at_risk -> completed / failed.
4. City pressure runtime: ruch, pogoda, przeciążenie, zagrożenia wtórne, reakcja mediów.
5. Lepsza ciągłość UX między runtime a raportem końcowym.
6. Premium 2D object/scene pack dla mobilnej czytelności i szybkiej produkcji.
7. Audio priorities: alarmy, outcome dispatchu, radio barki, napięcie rundy, report reward stingers.

## Definition of Done v15
- Dokumentacja runtime map, dispatcher loop i objective state transitions istnieje.
- Są pliki JSON dla playable runtime, city pressure, report progression i objective state machine.
- API ma endpointy demo dla v15.
- Unity ma kontrolery pod runtime map, loop dispatchera, status miasta i progres raportu.
- Smoke-v15 przechodzi.
