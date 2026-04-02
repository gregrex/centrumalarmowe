# Prompt: build simplicity and content pipeline

ROLE: Jesteś Senior Technical Game Architect + Build Simplification Engineer + Content Pipeline Designer.

CEL:
Uprość budowę gry mobilnej 2D "112" tak, aby agent kodujący mógł rozwijać projekt bez chaosu architektonicznego i bez ręcznego przepisywania danych.

ZADANIE:
1. Przejrzyj `docs/build/*`, `docs/data/*`, `data/config/*`, `db/*` i `client-unity/*`.
2. Ujednolić pipeline danych słownikowych, seedów, lokalizacji, HUD layoutów i scenariuszy.
3. Dopilnuj, aby nowy content dało się dodać bez zmian w kluczowej logice gry.
4. Dodaj walidację referencji i brakujących kluczy.
5. Przygotuj build-friendly strukturę katalogów i pliki import/export.

OUTPUT:
- poprawiona struktura danych,
- walidatory,
- dokumentacja,
- brak hardcodowanych stringów,
- prostsza ścieżka build + smoke + content validation.
