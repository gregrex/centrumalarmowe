# 01 — Architektura Aktualna: 112 Centrum Alarmowe

> **Wersja:** v26 | **Stack:** .NET 10 + SignalR + Unity 2D

---

## Diagram warstw

```
┌─────────────────────────────────────────────────────────┐
│                   CLIENT LAYER                          │
│   Unity 2D Mobile (client-unity/)  |  Admin Web Panel  │
│   SignalR WebSocket  |  REST HTTP                       │
└─────────────────────┬───────────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────┐
│                   API LAYER                             │
│   Alarm112.Api (ASP.NET Core Minimal API)               │
│   ├── /hubs/session   (SessionHub : SignalR)            │
│   ├── /api/*          (80+ minimal endpoints)           │
│   ├── /health         (liveness probe)                  │
│   ├── /auth/dev-token (JWT dev generator)               │
│   └── /swagger        (OpenAPI UI)                      │
└─────────────────────┬───────────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────┐
│               APPLICATION LAYER                         │
│   Alarm112.Application                                  │
│   ├── Interfaces/ (29 interfejsów)                      │
│   ├── Services/   (28 serwisów Singleton)               │
│   └── Factories/  (DemoFactory, VerticalSliceFactory)   │
└──────┬─────────────────────────────────┬───────────────┘
       │                                 │
┌──────▼──────────┐         ┌────────────▼───────────────┐
│  DOMAIN LAYER   │         │   CONTRACTS LAYER          │
│ Alarm112.Domain │         │   Alarm112.Contracts        │
│ ├── RoleType    │         │   (DTOs only, shared)       │
│ ├── SessionState│         │                             │
│ ├── IncidentCat.│         │                             │
│ └── Enums...    │         │                             │
└─────────────────┘         └────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────┐
│             INFRASTRUCTURE LAYER                        │
│   Alarm112.Infrastructure                               │
│   └── InMemorySessionStore (ConcurrentDictionary)       │
│       [docelowo: PostgreSQL via EF Core / Dapper]       │
└─────────────────────────────────────────────────────────┘
                      │
┌─────────────────────▼───────────────────────────────────┐
│               EXTERNAL SERVICES                         │
│   PostgreSQL 16  |  Redis 7  (docker-compose)           │
│   [aktualnie NIEUŻYWANE przez aplikację]                │
└─────────────────────────────────────────────────────────┘
```

---

## Zależności między projektami

```
Alarm112.Api
  → Alarm112.Application
  → Alarm112.Contracts
  → Alarm112.Infrastructure

Alarm112.Application
  → Alarm112.Domain
  → Alarm112.Contracts

Alarm112.Infrastructure
  → Alarm112.Application (interfejsy)
  → Alarm112.Contracts

Alarm112.AdminWeb
  → (standalone, komunikacja przez HTTP z API)

Alarm112.Domain
  → (brak zależności zewnętrznych)

Alarm112.Contracts
  → (brak zależności zewnętrznych)
```

---

## Cykl życia sesji

```
Draft ──► Lobby ──► Countdown ──► Active ──► Recovery ──► Summary ──► Archived
```

Każde przejście stanu wysyłane jest przez `RealtimeEnvelopeDto` via SignalR.

---

## Rejestracja serwisów (DI)

Wszystkie serwisy jako `Singleton`. Wymaga thread-safe implementacji.

| Serwis | Interfejs | Odpowiedzialność |
|--------|-----------|-----------------|
| SessionService | ISessionService | CRUD sesji gry |
| BotDirector | IBotDirector | AI fallback dla ról |
| BotTickHostedService | IHostedService | Cykliczny tick botów (2s) |
| LobbyService | ILobbyService | Zarządzanie lobby |
| CityMapService | ICityMapService | Mapa + dispatch |
| OperationsBoardService | IOperationsBoardService | Tablica incydentów |
| RoundRuntimeService | IRoundRuntimeService | Runtime rundy |
| QuickPlayService | IQuickPlayService | Szybka rozgrywka |
| ReferenceDataService | IReferenceDataService | JSON data bundles |
| ContentValidationService | IContentValidationService | Walidacja JSON |
| MissionFlowService | IMissionFlowService | Przepływ misji |
| MissionRuntimeService | IMissionRuntimeService | Runtime misji |
| PlayableRuntimeService | IPlayableRuntimeService | Grywalny runtime |
| ... (15+ kolejnych) | ... | Dane demo/release |

---

## SignalR Hub

**Endpoint:** `/hubs/session`  
**Klasa:** `SessionHub : Hub`

| Metoda klienta → serwer | Opis |
|-------------------------|------|
| `JoinSession(sessionId)` | Dołącz do grupy SignalR |
| `LeaveSession(sessionId)` | Opuść grupę |
| `Heartbeat(sessionId, role)` | Ping keepalive |

| Zdarzenie serwer → klient | Opis |
|---------------------------|------|
| `session.joined` | Potwierdzenie dołączenia |
| `session.left` | Potwierdzenie wyjścia |
| `session.heartbeat.ack` | Odpowiedź na ping |
| `session.envelope` | Koperta zdarzenia realtime |

---

## Middleware Pipeline

```
Request
  └─► UseExceptionHandler (ProblemDetails)
  └─► UseStatusCodePages
  └─► UseSwagger / UseSwaggerUI
  └─► UseCors
  └─► UseRateLimiter
  └─► UseAuthentication (JWT)
  └─► UseAuthorization
  └─► [Conditional] Auth enforcement middleware
  └─► Endpoints (MapGet/MapPost/MapHub)
```

---

## Dane content-driven

```
data/
├── config/          # roles.json, bot_profiles.json, hud_layouts.json
├── content/         # katalogi incydentów, jednostki, misje
├── reference/       # reference-data.vXX.json (bundlowane ładunki API)
├── ui/              # teksty UI (pl-PL), panele HUD
├── balance/         # parametry balansu gry
├── art/             # asety graficzne
└── audio/           # konfiguracja audio
```

Ładowane przez `IReferenceDataService` / `IContentBundleLoader` (`JsonContentBundleLoader`).

---

## Schemat bazy danych

21 migracji SQL w `db/schema/`:
- `001_init.sql` — bazowe tabele (players, sessions, incident_templates, telemetry_events)
- `002–021` — rozszerzenia per wersja (v5–v26)

**UWAGA:** Migracje istnieją ale PostgreSQL nie jest podłączony do aplikacji.  
Aplikacja używa `InMemorySessionStore`.

---

## Konfiguracja środowisk

| Plik | Środowisko | Uwagi |
|------|------------|-------|
| `appsettings.json` | Domyślna | Dev key, RequireAuth=false |
| `appsettings.Development.json` | Development | (brak pliku) |
| `appsettings.Production.json` | Production | (brak pliku — KRYTYCZNE) |
| `.env.example` | Lokalne | Tylko porty |
| `infra/docker-compose.yml` | Docker Dev | Hardcoded credentials |
