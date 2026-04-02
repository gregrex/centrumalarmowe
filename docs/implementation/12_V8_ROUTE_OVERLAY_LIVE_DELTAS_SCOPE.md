# V8 scope: route overlay na mapie, live delty i półgrywalna runda coop

## Cel wersji
Wersja v8 ma połączyć dotychczasowe elementy w bardziej namacalną rundę coop 2–4 graczy:
- mapa miasta nie tylko pokazuje incydenty, ale też trasę i obciążenie,
- jednostki mają runtime status, ETA i cooldown,
- ekran roli może działać jako osobny panel albo split panel,
- zdarzenia aktualizują się przez lekkie delty,
- boty reagują na delty i mogą przejąć brakującego gracza.

## Główne filary v8
1. **Route overlay on map** – przebieg trasy ma być widoczny bez otwierania osobnego okna.
2. **Live incident deltas** – zmiany stanu incydentu mają być czytelne jako małe komunikaty, nie pełne odświeżenie wszystkiego.
3. **Split-screen role panels** – jeden telefon/tablet może w debug buildzie lub lokalnym coop pokazać dwa role-panels.
4. **Unit runtime list** – dostępność jednostek ma zależeć od cooldownów i ETA, nie tylko od flagi available.
5. **Half-playable round loop** – minimum jedna runda ma się dać przejść jako: alarm -> analiza -> wybór jednostki -> overlay trasy -> dispatch -> delta -> wynik cząstkowy.
6. **Role bots v4** – bot ma reagować na pogorszenia i opóźnienia, a nie tylko wykonywać timer-based fallback.

## Zakres MVP v8
- 1 mapa miasta,
- 4 role,
- do 8 aktywnych incydentów,
- do 12 jednostek runtime,
- 3 typy route overlay style,
- 4 typy delt incydentu,
- 2 układy split paneli,
- 1 półgrywalna runda coop z bot-fill.

## Definition of Done v8
- `tools/content-verify.sh` przechodzi,
- `tools/smoke-v8.sh` przechodzi,
- nowe JSON-y v8 są poprawne,
- API ma endpointy route overlay / round state / live deltas / units runtime,
- klient Unity ma starter kontrolery v8,
- dokumentacja gameplay/UI/backend/audio/art/testy jest spójna.
