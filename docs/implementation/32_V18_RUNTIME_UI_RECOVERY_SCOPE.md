# V18 Runtime UI + Recovery Scope

## Cel
Wersja v18 podnosi vertical slice z „quasi-produkcyjnego demo” do bardziej namacalnego runtime UI jednej misji:
- recovery cards pojawiają się bezpośrednio w HUD,
- fail / retry / next jest częścią spójnego flow,
- jedna demo-misja ma mocniejszy polish grafiki 2D, obiektów, scen, audio i muzyki,
- agent implementacyjny dostaje prostszy, bardziej praktyczny zestaw zadań.

## Zakres
1. Runtime HUD dla jednej misji.
2. Aktywne use-case dla recovery cards:
   - traffic jam,
   - blocked lane,
   - overload,
   - missing ambulance,
   - radio blackout.
3. Fail / Retry / Next flow:
   - immediate fail branch,
   - partial success branch,
   - retry with hints,
   - next mission gate.
4. Premium 2D content pack:
   - route overlays,
   - city markers,
   - incident cards,
   - status badges,
   - failstate scenes,
   - retry transition cards.

## Bramy jakości
- HUD ma czytelne priorytety informacji.
- Recovery cards nie blokują najważniejszych ostrzeżeń.
- Retry flow ma być szybki i zrozumiały.
- Dane pozostają content-driven.
