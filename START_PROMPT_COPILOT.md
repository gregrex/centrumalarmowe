# START PROMPT — GitHub Copilot

## Kontekst
Budujesz grę mobilną 2D „112: Centrum Alarmowe”.
To gra real-time management / dispatch / coop do 4 graczy, w której gracze zarządzają zdarzeniami miejskimi przez centrum alarmowe.
Produkt ma mieć:
- kampanię,
- szybkie sesje,
- sandbox,
- tryb coop do 4 graczy,
- AI boty zastępujące brakujących graczy,
- backend sesji,
- panel admina,
- liveops i content pipeline.

## Twoje zadanie
1. Przeczytaj wszystkie pliki `.md`, `.json`, `.sql`, `.yml`.
2. Zbuduj plan wykonawczy moduł po module.
3. Ustal MVP, vertical slice i production scope.
4. Realizuj kroki z `docs/implementation/1000_STEPS_MASTER_INDEX.md`.
5. Po każdym etapie:
   - zaktualizuj dokumentację,
   - uruchom build/test,
   - odnotuj status.

## Kluczowe wymagania
- telefoniczny, pionowy UI,
- szybkie alerty i czytelne mapy,
- różne role coop,
- AI takeover na role bez człowieka,
- system dynamicznych zdarzeń,
- stabilna synchronizacja sieciowa,
- sensowne, realistyczne audio i grafika 2D,
- rozbudowany backend i panel admina.

## Start discovery
W pierwszym kroku wygeneruj:
- raport discovery,
- proponowaną strukturę repo kodowego,
- listę braków,
- kolejność implementacji,
- listę pierwszych 50 zadań wykonawczych.


DODATKOWY PRIORYTET V11:
- dokończ przepływ Home -> Chapter Map -> Mission Entry -> Offline/Online -> Bot Fill -> Round Start,
- zachowaj premium 2D mobile readability,
- wykorzystuj dane słownikowe i JSON content bundles zamiast hardcodowania.


## V20 focus
Priorytet: showcase demo package, onboarding FTUE, player-facing polish, capture mode, jedna misja pokazowa od Home do reward reveal/next.
