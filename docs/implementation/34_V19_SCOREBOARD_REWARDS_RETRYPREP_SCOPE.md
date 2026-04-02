# V19 — Runtime Scoreboard, Reward Reveal i Retry Preparation

## Cel
Dowieźć warstwę `near-final one-mission slice`, w której po zakończeniu rundy gracz widzi czytelny scoreboard, sekwencję reward reveal, przygotowanie do retry oraz płynny handoff do kolejnej misji lub powrotu do Home.

## Zakres
- runtime scoreboard z metrykami na żywo i końcowymi
- reward reveal states: xp, gwiazdki, badge, unlock teaser
- retry preparation: co poprawić, jaki loadout, który bot ma przejąć rolę
- next mission handoff: hook fabularny, rekomendowany rozdział, suggested squad
- spójność art/audio/menu/runtime/report

## Minimalny efekt
Home -> Mission -> Runtime -> Report -> Scoreboard -> Reward Reveal -> Retry Prep / Next Mission.

## Kryteria jakości
- czytelność na telefonie w pionie
- mała liczba kliknięć
- jednoznaczne stany success/partial/fail
- content-driven konfiguracja progów i nagród
