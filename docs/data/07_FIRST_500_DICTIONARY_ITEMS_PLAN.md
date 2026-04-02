# First 500 dictionary items plan

## Cel
Przygotować bezpieczny plan rozszerzania contentu bez hardcodu.

## Grupy rekordów
- 40 ról, slotów i polityk sesji,
- 80 typów incydentów,
- 60 typów jednostek i wariantów stanów,
- 80 stanów gameplayowych i alertów,
- 70 kluczy HUD i ekranów,
- 60 kluczy audio i muzyki,
- 50 wpisów art catalog,
- 60 wpisów bot logic / reason codes.

## Strategia
- najpierw rekordy rdzeniowe do vertical slice,
- potem rekordy rozszerzające regrywalność,
- potem liveops i seasonal.

## Notacja
- `incident.medical.collapse`
- `incident.fire.apartment`
- `unit.fire.engine.standard`
- `ui.screen.lobby.title`
- `alert.city.overload.critical`
- `bot.reason.no_human_in_slot`

## Quality gate
Nie próbuj od razu ręcznie wypełniać wszystkich 500 rekordów produkcyjnych. Najpierw:
- 120 rekordów działających,
- 200 rekordów draft,
- 500 rekordów planowanych w katalogu.
