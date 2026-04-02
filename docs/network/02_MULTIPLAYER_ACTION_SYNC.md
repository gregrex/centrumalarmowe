# Synchronizacja akcji

## Klasy akcji
- lokalne UI,
- komendy potwierdzane przez serwer,
- eventy ogłaszane wszystkim,
- aktualizacje stanu jednostek,
- wyniki director.

## Akcje krytyczne
- przydział jednostki,
- zmiana priorytetu zdarzenia,
- aktywacja procedury kryzysowej,
- takeover roli,
- zakończenie sesji.

## Wymagania
- każdy komunikat ma correlation id,
- każde polecenie ma timestamp i source actor,
- serwer rozstrzyga konflikty,
- klient potrafi odtworzyć state po reconnect.
