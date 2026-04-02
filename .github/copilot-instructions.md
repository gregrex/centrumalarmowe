# Copilot Instructions — 112: Centrum Alarmowe

## Projekt
Gra mobilna 2D real-time dispatch/management coop dla 4 graczy — „112: Centrum Alarmowe".
Backlog implementacyjny: `docs/implementation/1000_STEPS_MASTER_INDEX.md`.
Zawsze zaczynaj od `START_PROMPT_COPILOT.md` → `docs/implementation/00_MASTER_IMPLEMENTATION_PLAN.md`.

---

## Komendy build / test / smoke

```powershell
# Build całego solution
dotnet build Alarm112.sln

# Uruchom API lokalnie
dotnet run --project src/Alarm112.Api

# Walidacja JSON content bundli
.\tools\content-verify.ps1

# Smoke test bieżącej wersji (v26)
.\tools\smoke-v26.ps1

# Lokalny boot (weryfikacja + instrukcja startu)
.\tools\boot-local.ps1

# Infrastruktura (PostgreSQL + Redis)
docker compose -f infra/docker-compose.yml up -d

# Swagger UI (po starcie API)
# http://localhost:5080/swagger
```

Smoke testy dla każdej wersji są w `tools/smoke-vXX.ps1` i `tools/smoke-vXX.sh`.

---

## Architektura wysokopoziomowa

### Backend (.NET 10, ASP.NET Core + SignalR)
```
Alarm112.Domain        → enums i modele domeny (RoleType, SessionState, IncidentCategory…)
Alarm112.Contracts     → DTOs – jedyne obiekty przesyłane między warstwami i do klienta
Alarm112.Application   → interfejsy + serwisy (logika biznesowa, bez frameworku)
Alarm112.Infrastructure → persistence (InMemorySessionStore, docelowo PostgreSQL)
Alarm112.Api           → minimal API endpoints + SignalR hub (SessionHub)
Alarm112.AdminWeb      → panel admina (Blazor)
```

Zależności idą jednostronnie: Api → Application → Domain. Contracts to osobna warstwa współdzielona przez wszystkich.

### Klient (Unity 2D, mobile portrait)
`client-unity/Assets/Scripts/Runtime/` — podkatalogi: Core, Game, UI, Networking, Session, Map, Lobby, Menu, Bots, Coop, QuickPlay, Config, Art, Audio.

### Infrastruktura
- PostgreSQL 16 — stan gry i sesji
- Redis 7 — real-time pub/sub i cache
- SignalR (`/hubs/session`) — push zdarzeń do klientów Unity

### Dane (content-driven)
- `data/config/` — role, AI boty, HUD layouts, reguły directora
- `data/content/` — katalogi incydentów, jednostki, misje, mapy
- `data/reference/` — reference-data bundles ładowane przez `IReferenceDataService`
- `data/ui/` — teksty UI (pl-PL), panele HUD, splitscreen
- `data/balance/`, `data/art/`, `data/audio/` — pozostałe asety konfiguracyjne

### Baza danych
Migracje SQL w `db/schema/` numerowane `001_init.sql` … `021_v26_…sql` — stosuj je w kolejności.

---

## Kluczowe konwencje

### Warstwa DTO (Contracts)
- Każdy przepływ UI ma własne DTO w `Alarm112.Contracts` (np. `MissionBriefingDto`, `RolePanelStateDto`).
- DTOs nigdy nie zawierają logiki — tylko dane.
- Nie przekazuj obiektów domenowych poza warstwę Application.

### Serwisy jako Singleton
Wszystkie serwisy rejestrowane są jako `AddSingleton` w `Program.cs`. Serwisy muszą być thread-safe.
`BotTickHostedService` jest hosted service odpowiedzialnym za cykliczne ticki botów.

### Role i AI fallback
Cztery role: `CallOperator`, `Dispatcher`, `OperationsCoordinator`, `CrisisOfficer` — każda z osobnym HUD-em i profilem bota w `data/config/roles.json` i `data/config/bot_profiles.json`.
Jeśli gracz odejdzie, `IBotDirector` przejmuje jego rolę. Bot fallback **musi** działać dla każdej roli.

### Cykl sesji
`Draft → Lobby → Countdown → Active → Recovery → Summary → Archived`
Eventy real-time opakowuj w `RealtimeEnvelopeDto` i wysyłaj przez `SessionHub`.
Wszystkie akcje gracza mają być idempotentne (pole `ActionId` w `SessionActionDto`).

### SignalR
Hub pod `/hubs/session`. Klient wywołuje `JoinSession(sessionId)`, serwer wysyła `"session.envelope"` i `"session.heartbeat.ack"`.

### Content pipeline
- **Nigdy nie hardcoduj** danych scenariuszy, tekstów ani konfiguracji ról bezpośrednio w kodzie C# lub Unity.
- Dane ładuj z JSON bundli przez `IReferenceDataService` lub odpowiedni serwis aplikacyjny.
- Nowe typy zdarzeń/misji dodawaj do plików w `data/content/` i do odpowiedniego `reference-data.vXX.json`.

### Mapa ekranów
Przed dodaniem nowego ekranu sprawdź `docs/ui/00_SCREEN_MAP.md`. Nie buduj ekranów spoza mapy bez aktualizacji dokumentacji.

### Zasady niedozwolone
- Nie mieszaj logiki gry z warstwą prezentacji (UI).
- Nie pomijaj obsługi AI fallback dla żadnej roli.
- Nie modyfikuj `Alarm112.Domain` bez aktualizacji odpowiednich DTO w `Alarm112.Contracts`.

---

## Źródła prawdy (kolejność priorytetu)
1. `docs/implementation/` — szczegółowe scope i zadania per wersja
2. `docs/network/` — architektura real-time i synchronizacja
3. `docs/ui/` — mapa ekranów i HUD role
4. `docs/ai/` — AI bot takeover i profile
5. `data/config/` — konfiguracja runtime

