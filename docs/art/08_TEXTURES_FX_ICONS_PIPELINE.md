# Textures, FX and icon pipeline

## Tekstury
Gra 2D powinna opierać się na ograniczonej bibliotece materiałów:
- asfalt suchy,
- asfalt mokry,
- dachy,
- beton miejski,
- zieleń miejska,
- woda,
- światła nocne,
- gradient alarmowy,
- szum technologiczny HUD,
- noise pogodowy.

## Ikony
Każda ikona musi istnieć w wersjach:
- normal,
- selected,
- disabled,
- warning,
- critical.

## VFX
Minimalny zestaw premium VFX:
- ping lokalizacji,
- linia przejazdu,
- flare syren,
- puls strefy ryzyka,
- alarm globalny,
- awaria łączności,
- takeover bota,
- potwierdzenie dispatchu,
- fail action.

## Pipeline łatwy w budowie
1. AI reference / moodboard.
2. Manual cleanup i styl-guide lock.
3. Batch assetów SVG/PNG.
4. Atlasowanie.
5. Import do Unity przez foldery tematyczne.
6. Test czytelności na urządzeniu mobilnym.
7. Akceptacja lub cofnięcie.

## Wymagane katalogi assetów
- `ui/icons/`
- `ui/alerts/`
- `ui/portraits/`
- `map/tiles/`
- `map/buildings/`
- `map/weather/`
- `fx/`
