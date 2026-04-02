# FULL MISSION RUNTIME DISPATCH SCREEN

## Cel ekranu
Dać graczowi jeden centralny ekran runtime, na którym może:
- widzieć aktywne incydenty,
- wybierać jednostki,
- podejmować decyzje dispatch,
- obserwować outcome,
- kontrolować postęp misji.

## Główne strefy
1. Top bar
   - mission title
   - pressure state
   - city stability
   - mission timer
2. Left panel
   - active incidents
   - selected incident card
   - event feed preview
3. Center
   - city map
   - route overlay
   - incident markers
   - unit markers
4. Right panel
   - live units
   - availability
   - ETA
   - cooldown
   - dispatch CTA
5. Bottom strip
   - objective tracker
   - warnings
   - quick role hints

## Wymogi mobile 2D
- touch targets minimum 56 px
- priorytet informacji nad dekoracją
- jeden kolor alarmowy na jedną klasę problemu
- czytelne badge ETA i cooldown
- max 3 kliknięcia do dispatch

## Edge cases
- brak wolnej jednostki
- zbyt długa trasa
- konflikt priorytetu
- przeciążenie feedu
- misja zbliża się do fail gate
