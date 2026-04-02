# V26 — real Android build + bugfix freeze scope

## Cel wersji
Dowieźć paczkę, którą agent kodujący może potraktować jako ostatni etap przed pierwszym rzeczywistym buildem Android preview/internal.

## Zakres
- stabilizacja jednej misji showcase,
- ograniczenie zmian do krytycznych bugfixów i polishu,
- dopięcie operator + dispatcher jako głównego flow demo,
- przygotowanie checklist i artefaktów pod realny build Android,
- utrzymanie content-driven architektury,
- poprawa czytelności 2D mobile przy 720p i 1080p.

## Poza zakresem
- nowe tryby gry,
- nowe misje poza showcase,
- przebudowa modelu sieciowego,
- skok technologiczny w silniku.

## Bramy jakości
1. Home -> Briefing -> Runtime -> Report działa bez ślepych ekranów.
2. Operator i dispatcher mają czytelne HUD-y w pionie.
3. Smoke + content verify przechodzą.
4. Bugfix freeze blokuje zmiany o niskim priorytecie.
5. Pack demo jest gotowy do nagrania i pokazania.
