# Dictionaries master

## Cel
Cały projekt ma opierać się na danych słownikowych, nie na porozrzucanych stringach w kodzie.

## Zasady
- każdy enum biznesowy ma odpowiednik słownikowy dla contentu,
- każdy identyfikator ma format stabilny i wersjonowany,
- UI pobiera nazwy i opisy z kluczy tekstowych,
- content designer może dodać nowe rekordy bez kompilowania kodu,
- seed SQL i JSON muszą odnosić się do tych samych kluczy.

## Główne słowniki
- role,
- typy jednostek,
- kategorie incydentów,
- poziomy ciężkości,
- statusy zgłoszeń,
- statusy jednostek,
- typy pogody,
- mutatory sesji,
- profile botów,
- stany audio,
- typy alertów,
- stany HUD,
- archetypy postaci,
- style graficzne,
- klucze lokalizacji.

## Format ID
- `role.operator`
- `incident.fire.apartment`
- `unit.ambulance.basic`
- `status.incident.waiting_dispatch`
- `audio.state.critical`
- `portrait.operator.senior.female.a`

## Quality gate
Żaden nowy rekord contentu nie przechodzi review, jeśli nie ma:
- stabilnego ID,
- nazwy technicznej,
- opisu,
- ownera,
- wersji,
- powiązania z lokalizacją,
- walidacji.
