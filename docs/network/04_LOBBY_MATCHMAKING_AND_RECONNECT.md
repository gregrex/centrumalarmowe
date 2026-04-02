# Lobby, matchmaking and reconnect

## Cel
Dać bardzo prosty, stabilny i łatwy do zbudowania model wejścia do sesji mobilnej 2D. Lobby ma działać dobrze zarówno dla gracza solo, jak i dla 2-4 graczy oraz przy botach zastępujących brakujące osoby.

## Zasady projektowe
- najpierw stabilność i czytelność, potem bajery,
- jedna sesja = jeden host logiczny po stronie serwera,
- wszystkie role są jawne i widoczne przed startem,
- brakująca rola może zostać obsadzona przez bota,
- reconnect nie może niszczyć sesji,
- join-in-progress tylko do bezpiecznego momentu misji.

## Typy wejścia
### Quick Play
- szybkie utworzenie sesji demo,
- domyślnie 1 gracz + boty,
- idealne do testów i vertical slice.

### Private Lobby
- kod pokoju,
- zaproszenia,
- ręczne przypisywanie ról,
- start przez hosta.

### Matchmaking
- kolejka publiczna,
- dobór po regionie, języku i preferowanej roli,
- fallback do botów po czasie oczekiwania.

## Model lobby
### Stany lobby
- Draft
- WaitingForPlayers
- ReadyCheck
- LoadingScenario
- InGame
- ReconnectWindow
- Finished
- Abandoned

### Sloty
- operator,
- dispatcher,
- coordinator,
- crisis_officer.

Każdy slot ma:
- occupant type: Human / Bot / Empty,
- display name,
- connection status,
- preferred language,
- readiness,
- takeover priority.

## Reconnect
### Okno reconnect
- 30 do 90 sekund, konfigurowalne per tryb,
- rola pozostaje zarezerwowana,
- bot może wejść tymczasowo,
- po powrocie człowiek może przejąć sterowanie.

### Zasada takeover
- jeśli rola odpadnie w czasie spokojnym, bot przejmuje płynnie,
- jeśli rola odpadnie w czasie krytycznym, bot przejmuje natychmiast z flagą `bot_emergency_takeover`,
- gdy gracz wróci, dostaje overlay: `Przejmij rolę / Pozostań obserwatorem przez 10 s`.

## Matchmaking minimalny do MVP
- brak rankingów,
- brak skomplikowanego MMR,
- tylko:
  - region,
  - język,
  - preferowana rola,
  - tryb sesji,
  - poziom trudności.

## Matchmaking do Production
- soft role balancing,
- unikanie duplikacji tej samej preferencji,
- skill buckets,
- fill by bot threshold,
- join late only for observer or backup operator.

## Edge cases
- host wychodzi przed startem,
- dwóch graczy chce tę samą rolę,
- brak kompletu po timeout,
- przerwany loading,
- reconnect po zmianie sieci,
- gracz wraca na innym urządzeniu,
- bot przejął za dużo akcji i człowiek wraca do roli.

## KPI
- czas wejścia do sesji,
- procent sesji startujących w mniej niż 60 s,
- procent reconnectów zakończonych sukcesem,
- procent sesji z bot fill,
- procent porzuceń przed startem.
