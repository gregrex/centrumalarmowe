# ARCHITECTURE.md — Architektura systemu

## Przegląd

**112: Centrum Alarmowe** to gra mobilna 2D real-time dispatch/management coop dla 4 graczy. Architektura opiera się na backendzie .NET 10 z SignalR do komunikacji real-time oraz kliencie Unity 2D.

---

## Diagram modułów

```
┌─────────────────────────────────────────────────────────┐
│                     Klient Unity                        │
│   Menu ─ Lobby ─ Map ─ Session ─ HUD ─ Bots           │
└────────────────┬────────────────────────────────────────┘
                 │  HTTP REST + SignalR WebSocket
┌────────────────▼────────────────────────────────────────┐
│                   Alarm112.Api                          │
│   Minimal API Endpoints (40+) + SignalR SessionHub     │
└──────┬──────────────────────────────┬───────────────────┘
       │                              │
┌──────▼──────────┐         ┌─────────▼───────────────────┐
│ Alarm112.       │         │ Alarm112.Infrastructure      │
│ Application     │         │  InMemorySessionStore        │
│  Services:      │         │  (ConcurrentDictionary)      │
│  SessionService │         └─────────────────────────────┘
│  BotDirector    │
│  LobbyService   │
│  ReferenceData  │
│  ...28 serwisów │
└──────┬──────────┘
       │
┌──────▼──────────┐    ┌─────────────────────────────────┐
│ Alarm112.Domain │    │ Alarm112.Contracts (DTOs)        │
│  RoleType       │    │  SessionSnapshotDto              │
│  SessionState   │    │  SessionActionDto                │
│  IncidentCategory│    │  RealtimeEnvelopeDto             │
│  ...enums       │    │  ...wszystkie DTO                │
└─────────────────┘    └─────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                   Alarm112.AdminWeb                     │
│   Dark-theme HTML dashboard, health, API card links    │
└─────────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────────┐
│                   Infrastruktura                        │
│   PostgreSQL 16 (stan sesji) │ Redis 7 (pub/sub, cache)│
└─────────────────────────────────────────────────────────┘
```

---

## Warstwy i zależności

Zależności **jednokierunkowe**:

```
Alarm112.Api
    → Alarm112.Application
        → Alarm112.Domain
        → Alarm112.Contracts
    → Alarm112.Infrastructure
    → Alarm112.Contracts
Alarm112.AdminWeb (oddzielny serwis HTTP)
```

**Zasada**: żadna warstwa nie może importować warstwy powyżej siebie.

---

## Przepływ danych — sesja gry

```
Unity Client
  │
  ├── POST /sessions/demo         → SessionService.CreateDemoSessionAsync()
  │                                  → DemoFactory.Create()
  │                                  → ISessionStore.Save()
  │
  ├── GET  /sessions/{id}/snapshot → SessionService.GetSnapshotAsync()
  │                                  → ISessionStore.GetOrAdd()
  │
  ├── POST /sessions/{id}/action   → SessionService.ApplyActionAsync()
  │       { actionType: "dispatch", payloadJson: {...} }
  │                                  → ApplyDispatch/Escalate/Resolve
  │                                  → ISessionStore.Save()
  │
  └── SignalR /hubs/session
        JoinSession(sessionId)
        ← "session.envelope" (RealtimeEnvelopeDto)
        ← "session.heartbeat.ack"
```

---

## Serwisy aplikacyjne (Alarm112.Application)

| Serwis | Odpowiedzialność |
|---|---|
| `SessionService` | Tworzenie/odczyt sesji, dispatch akcji graczy |
| `BotDirector` | AI fallback — przejmuje rolę gracza, wywołuje ApplyActionAsync |
| `LobbyService` | Zarządzanie lobby przed startem sesji |
| `ReferenceDataService` | Ładowanie reference-data bundli JSON |
| `ContentValidationService` | Walidacja wymaganych plików i bundli |
| `QuickPlayService` | Szybkie dołączenie do sesji |
| `CityMapService` | Dane mapy miasta i markerów |
| `OperationsBoardService` | Tablica operacyjna (incydenty, jednostki) |
| `MissionFlowService` | Przepływ misji |
| `HomeFlowService` | Ekrany home/hub |
| `CampaignEntryService` | Wejście do kampanii |
| `ThemePackService` | Motywy graficzne / paczki |
| `RuntimeBootstrapService` | Bootstrap runtime sesji |
| ...i 15 dalszych | Pokrywają wszystkie przepływy UI z docs/ui/ |

---

## Cykl sesji

```
Draft → Lobby → Countdown → Active → Recovery → Summary → Archived
```

- Każda zmiana stanu jest wysyłana przez SignalR jako `RealtimeEnvelopeDto`
- Zdarzenia pakowane w `session.envelope` 
- Akcje gracza są **idempotentne** (pole `ActionId` w `SessionActionDto`)

---

## Role w grze

| Rola | Opis |
|---|---|
| `CallOperator` | Odbiera zgłoszenia, triaguje incydenty |
| `Dispatcher` | Wysyła jednostki ratunkowe |
| `OperationsCoordinator` | Koordynuje zasoby |
| `CrisisOfficer` | Zarządza kryzysem, eskaluje |

Każda rola ma osobny HUD i profil bota w `data/config/bot_profiles.json`. Jeśli gracz odejdzie, `IBotDirector` przejmuje jego rolę.

---

## Content pipeline

```
data/
  config/        ← konfiguracja ról, botów, HUD layouts
  content/       ← katalogi incydentów, jednostki, misje, mapy
  reference/     ← bundles ładowane przez IReferenceDataService
  ui/            ← teksty UI (pl-PL), panele HUD, splitscreen
  balance/       ← parametry balansu
  art/, audio/   ← aset-konfiguracyjne
```

**Zasada**: nigdy nie hardcoduj danych gry w C#. Wszystko musi być w JSON i ładowane przez `IContentBundleLoader` lub `IReferenceDataService`.

---

## Infrastruktura

| Komponent | Rola | Port domyślny |
|---|---|---|
| PostgreSQL 16 | Stan sesji i gry (docelowo) | 5432 |
| Redis 7 | Real-time pub/sub, cache | 6379 |
| SignalR Hub | Push zdarzeń do Unity | /hubs/session |

Aktualnie `InMemorySessionStore` zastępuje PostgreSQL (in-process, thread-safe). Swap na DB-backed wymaga tylko zmiany rejestracji DI.

---

## Klient Unity

```
client-unity/Assets/Scripts/Runtime/
  Core/        — wejście i zarządzanie cyklem życia
  Game/        — logika gry
  Session/     — sesja coop
  Map/         — mapa i markery
  UI/          — warstwy UI
  Networking/  — HTTP + SignalR client
  Lobby/       — lobby przed sesją
  Bots/        — fallback bot UI
  ...19 katalogów, 132 skrypty
```

---

## Bezpieczeństwo

- **CORS**: skonfigurowany z listą origin (Cors:AllowedOrigins)
- **Rate limiting**: 200 req/10s per IP (fixed window)
- **ProblemDetails**: ustandaryzowane odpowiedzi błędów (RFC 7807)
- **Exception handler**: globalna obsługa wyjątków bez ujawniania stack trace
