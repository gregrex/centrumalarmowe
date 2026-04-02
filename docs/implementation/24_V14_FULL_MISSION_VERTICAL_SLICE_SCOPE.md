# V14 FULL MISSION VERTICAL SLICE SCOPE

## Cel
Zbudować pierwszy spójny vertical slice jednej pełnej misji:

`Home -> Chapter Map -> Mission Entry -> Briefing -> Team Readiness -> Runtime Dispatch -> Mission Complete Gate -> Post-Round Report`

Ta wersja ma być dalej łatwa do budowy przez agenta, dlatego:
- runtime jest kontrolowany przez dane,
- UI jest oparte na prostych panelach 2D mobile,
- grafika 2D premium ma mieć małą liczbę warstw i czytelne siluety,
- audio i muzyka mają mieć mało stanów, ale duży wpływ na napięcie.

## Zakres V14
1. Jedna pełna misja z kontrolowanym scenariuszem.
2. Prosty runtime dispatch dla trzech klas incydentów:
   - medyczny,
   - pożarowy,
   - policyjny.
3. Gate ukończenia misji oparty na:
   - czasie,
   - minimalnych objective states,
   - stanie miasta,
   - gotowości zespołu.
4. Raport po misji z:
   - oceną,
   - gwiazdkami,
   - nagrodami,
   - błędami,
   - timeline summary.
5. Spójność wizualna między:
   - menu,
   - briefingiem,
   - runtime,
   - raportem.
6. Spójność audio:
   - menu ambience,
   - briefing pulse,
   - runtime pressure layers,
   - mission complete stinger,
   - report reveal.

## Co musi istnieć
- runtime state API,
- dispatch outcome API,
- objective tracker API,
- mission script API,
- mission complete gate API,
- Unity controllers dla runtime HUD,
- objective tracker i event feed,
- kontrolery gate/report transition,
- dane contentowe dla jednej pełnej misji,
- smoke test V14.

## Wymagania build-friendly
- jeden mission script w JSON,
- jedna paczka promptów dla scen i obiektów,
- jeden audio pack dla runtime,
- brak zależności od finalnych assetów produkcyjnych,
- placeholder icons i placeholder object cards wystarczą do pierwszego prototypu.

## Definition of Done
- smoke V14 przechodzi,
- wszystkie pliki V14 istnieją,
- API zwraca payloady demonstracyjne,
- klient ma kontrolery dla runtime i gate flow,
- dokumentacja opisuje grafiki, obiekty, sceny, menu, dźwięki i muzykę.
