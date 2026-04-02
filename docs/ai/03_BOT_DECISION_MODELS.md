# Bot Decision Models

## Założenie
Bot nie ma być wszechwiedzącym cheatem. Ma być poprawnym zastępcą brakującego gracza.

## Typy botów
- conservative,
- balanced,
- aggressive,
- tutorial-helper.

## Model decyzji
1. Odczytaj slot roli i aktualny snapshot.
2. Zidentyfikuj akcje legalne.
3. Nadaj wagę akcji wg roli i priorytetu.
4. Odrzuć akcje wysokiego ryzyka bez potwierdzenia, jeśli bot jest conservative.
5. Wykonaj najwyżej ocenioną akcję po cooldownie.

## Cele jakości
- bot ma być przewidywalny,
- nie może spamować komend,
- musi zostawiać miejsce na powrót gracza,
- ma tłumaczyć swoje decyzje w logu debug.
