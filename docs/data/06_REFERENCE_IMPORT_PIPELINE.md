# Reference import pipeline

## Cel
Mieć jeden prosty, przewidywalny przepływ danych od autora contentu do gry.

## Przepływ
1. Autor edytuje JSON lub CSV źródłowy.
2. Skrypt waliduje strukturę i wymagane pola.
3. Skrypt buduje bundle referencyjny.
4. Bundle trafia do:
   - klienta jako read-only content,
   - backendu jako seed/import,
   - admina jako preview source.
5. Session bootstrap pobiera wersję bundle.

## Źródła danych
- `data/reference/*.json`
- `data/content/*.json`
- `data/ui/*.json`
- `data/art/*.json`

## Anti-chaos rules
- brak magicznych stringów w kodzie,
- wszystkie ID są w słownikach,
- wszystkie nazwy dla użytkownika z lokalizacji,
- wszystkie grafiki podpięte przez katalog assetów.

## Quality gate
Wprowadzenie nowego typu danych wymaga:
- dokumentu,
- schematu,
- przykładowego pliku,
- walidatora,
- mapowania do klienta.
