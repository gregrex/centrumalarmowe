# dokoca.md

## Raport discovery — "112: Centrum Alarmowe" (skrócony)

Cel projektu:
- Mobilna gra 2D (portrait) real-time management / dispatch / coop do 4 graczy.
- Tryby: kampania, szybkie sesje, sandbox, coop (4 graczy) z AI botami, backend sesji, panel admina, liveops.

Kluczowe wymagania (skondensowane):
- Telefoniczny, pionowy UI.
- Czytelne, szybkie alerty i mapy.
- Różne role coop; AI takeover na brakujących graczy.
- Dynamiczny system zdarzeń i stabilna synchronizacja sieciowa.
- Realistyczne audio, 2D grafika; rozbudowany backend + panel admina.

MVP (minimalny, funkcjonalny):
- Home -> Chapter Map -> Mission Entry -> Online/Offline -> Bot Fill -> Round Start flow działający end-to-end.
- Jedna pokazowa misja (vertical slice) z reward reveal i next.
- Lokalny tryb single-player z botami. Prosty backend sesji (rooms), podstawowy admin panel do tworzenia misji.

Vertical Slice (pokazowy):
- Pełny flow od Home do reward reveal dla jednej misji z: UI, bot-fill, network sync (hosted), prostą kampanią 1 rozdział.
- Onboarding FTUE i capture mode.

Production scope (pełna wersja):
- Kampania wielomisyjna, matchmaking, persistent progression, liveops + content pipeline, 4-player coop z AI botami dowolnej roli, rozbudowany panel admina.

Proponowana struktura repo (top-level):
- game/                      — kod klienta (Unity, Godot lub custom engine) z podfolderami `ui`, `gameplay`, `audio`, `assets`.
- server/                    — backend sesji (Node/Go/.NET) z `auth`, `rooms`, `events`, `matchmaking`.
- admin-panel/               — panel admina (React + API klienta do server/).
- bots/                      — logika AI botów + test harness.
- content/                   — JSON bundles, dictionary data, mission definitions, localization.
- assets/                    — grafika 2D, audio, atlasy, raw source.
- docs/                      — specyfikacje, pipelines, implementation/1000_STEPS_MASTER_INDEX.md
- tools/                     — skrypty do buildów, content-pipeline, importerów.
- ci/                        — konfiguracje CI/CD.
- build/                     — build outputs (gitignored).

Lista braków (najważniejsze):
1. Brak istniejącego pliku `dokoca.md` w repo (utworzono teraz).
2. Brak specyfikacji technicznej backendu (stack, API spec).
3. Brak wytycznych engine klienta (Unity vs Godot vs native).
4. Brak content bundles (misje w JSON): słowniki, event templates.
5. Brak testów sieciowych/synchronizacji i test harness dla botów.
6. Brak CI/CD konfiguracji i playtest harness.

Kolejność implementacji (wysoki priorytet):
1. Zdefiniować decyzje architektoniczne: engine klienta, backend stack, format content bundles.
2. Zaimplementować minimalny backend rooms + prosty protokół synchronizacji (host authoritative lub lockstep minimal).
3. Zaimplementować klienta vertical slice (Home->Mission->Start->Round->Reward) z lokalnym bot-fill.
4. Przygotować content bundle dla jednej misji pokazowej.
5. Stworzyć prosty admin-panel do ładowania misji i zarządzania kontentem.
6. Dodać AI boty prostego poziomu, testować w lokalnych sesjach.
7. Instrumentacja, telemetry, oraz podstawy liveops.

Zadania wykonawcze — pierwsze 50 (opisane krótką frazą):
1. Utworzyć plik `dokoca.md` i raport discovery (TEN PLIK).
2. Zdecydować silnik klienta (Unity/Godot) i zapisać uzasadnienie.
3. Wybrać stack backendu (Node/Go/.NET) i zapisać API style guide.
4. Zdefiniować format JSON content bundle (mission schema).
5. Scaffold repo: utworzyć foldery `game/ server/ admin-panel/ content/ bots/ assets/ docs/`.
6. Przygotować przykładowy content bundle dla 1 misji (JSON).
7. Implementować prosty lokalny host dla rooms (server minimal).
8. Dodać endpointy: createRoom, joinRoom, startRound, syncState.
9. Zaimplementować prosty klient UI: ekran Home (mockup).
10. Zaimplementować Chapter Map — nawigacja do misji.
11. Implementować Mission Entry UI — parametry misji i start.
12. Dodać tryb Bot Fill: fill brakujących graczy botami.
13. Implementować Round Start flow lokalnie (single-player + bots).
14. Renderować prostą mapę 2D i alert markers.
15. Implementować szybkie alert UI i priorytety powiadomień.
16. Stworzyć prosty system eventów losowych dla misji.
17. Implementować podstawową synchronizację stanu gry (server authoritative minimal).
18. Dodać prosty AI bot: podejmowanie decyzji na podstawie roli.
19. Napisać prosty test harness do symulacji 4 graczy z botami.
20. Utworzyć admin-panel skeleton (React) i połączyć z server.
21. Endpoint admin: upload mission JSON.
22. Endpoint admin: list missions, enable/disable.
23. Dodać system rewards i reward reveal UI.
24. Implementować onboarding FTUE (pierwsze uruchomienie).
25. Implementować capture mode (tryb demonstracyjny do rejestracji/pokazu).
26. Dodać logger/telemetry basic (events, errors).
27. Przygotować pipeline content (skrypt do walidacji JSON bundli).
28. Dodać prostą lokalizację (pliki słownikowe JSON).
29. Przygotować assets placeholder (grafika, audio) i standard eksportu.
30. Implementować prosty matchmaking (lokalny lub lobbies).
31. Dodać obsługę offline/online w UI misji.
32. Testować synchronizację przy 4 klientach (lokalny test harness).
33. Zaimplementować prostą walidację kontraktu API (schema tests).
34. Przygotować dokumentację deweloperską w `docs/`.
35. Dodać skrypt build klienta i servera (local run scripts).
36. Przygotować plan automatycznych testów E2E dla vertical slice.
37. Optymalizacja UI pod portrait mobile (fonty, spacing).
38. Dodać prostą obsługę audio dla alertów (SFX pipeline).
39. Dodać prostą animację reward reveal.
40. Zaimplementować retry/reziliency mechanizmy sieciowe.
41. Przygotować podstawy bezpieczeństwa API (auth minimal).
42. Przygotować prosty system kont graczy i progresji (local DB).
43. Dodać mock server dla testów offline.
44. Przeprowadzić pierwsze playtesty vertical slice i zebrać feedback.
45. Iterować na bot AI (poprawa decyzji i roli).
46. Dodać prostą konsolę admin do debugowania sesji live.
47. Przygotować manifesty do CI (build/test pipeline skeleton).
48. Stworzyć prosty system wersjonowania content bundles.
49. Przygotować checklistę release readiness dla vertical slice.
50. Zdefiniować roadmapę publikacji showcase demo i plan FTUE.

---

Status na teraz:
- Plik `dokoca.md` utworzony i Zadanie 1 wykonane (raport discovery i lista 50 zadań).

Decyzja silnika klienta:
- Wybrany silnik: Unity (2D, Mobile) — decyzja domyślna dla szybkiego prototypu oraz najszerszego wsparcia mobilnego i ekosystemu narzędzi.
- Uzasadnienie: Unity zapewnia szybki start dla 2D mobile, szerokie wsparcie platform, duży ekosystem assetów i narzędzi (profiling, remote build), oraz łatwiejszą integrację z usługami sieciowymi i CI/CD. Zalecane przechowywanie jedynie `Assets/` i `ProjectSettings/` w repo lub użycie oddzielnego repo dla dużych assetów + Git LFS.

Zadanie 2: decyzja silnika klienta zapisana (Unity) — wykonane.

Braki / prośba o potwierdzenie:
- Potwierdź wybór silnika klienta (Unity rekomendowane dla szybki start + mobilne wsparcie) — jeśli potwierdzasz, zacznę od scaffoldu Unity w `game/`.
- Potwierdź preferowany backend (najbardziej prawdopodobne: Node.js + WebSocket dla szybkiego prototypu). Jeśli zgadzasz, zacznę scaffold `server/`.

Następne kroki (wykonam domyślnie jeśli brak odpowiedzi):
1) Utworzyć scaffold repo i przykładowy mission JSON (najbardziej prawdopodobne — wykonam). 
2) Zaimplementować prosty lokalny server rooms (jeśli wolisz inną opcję, wybierz).
3) Rozpocząć klient vertical slice (Home->Mission->Start) w wybranym engine.

Jeżeli chcesz inny priorytet niż powyżej, napisz wybraną opcję (1/2/3...).
