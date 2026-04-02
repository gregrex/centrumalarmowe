# Build-friendly authoring rules

## Cel
Budowa gry ma być łatwa, przewidywalna i tania w utrzymaniu.

## Zasady tworzenia contentu
1. Jeden rekord = jedna odpowiedzialność.
2. Dane muszą dać się zmergować bez ręcznego grzebania.
3. Scenariusze mają małe pliki i include references, a nie jeden gigantyczny JSON.
4. Każdy asset ma ownera, status i powiązane ID.
5. Boty nie mają unikalnej logiki na sztywno per misja, tylko profile i reguły.
6. UI layout ma warianty danych, a nie osobny kod per rola, jeśli nie trzeba.
7. Feature flagi muszą umożliwiać wyłączanie ryzykownych elementów.

## Pakiety danych
- `reference/` stabilne słowniki,
- `scenarios/` misje,
- `balance/` tabele,
- `ui/` layouty i teksty,
- `art/` style i listy assetów,
- `audio/` mapowania cue.
