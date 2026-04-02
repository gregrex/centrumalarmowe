# 00 — Audyt Startowy: 112 Centrum Alarmowe

> **Data audytu:** Aktualna sesja robocza  
> **Wersja projektu:** v26 (Real Android Build)  
> **Audytor:** Autonomous Principal Architect Agent

---

## 1. Aktualny stan architektury

### Backend (.NET 10 preview)
- **Alarm112.Api** — minimal API (~962 loc) + SignalR hub, swagger, JWT rejestracja
- **Alarm112.Application** — 28 serwisów (wszystkie Singleton), 29 interfejsów
- **Alarm112.Contracts** — DTO only (brak walidacji!)
- **Alarm112.Infrastructure** — `InMemorySessionStore` (ConcurrentDictionary)
- **Alarm112.Domain** — enums + DemoFactory + VerticalSliceFactory
- **Alarm112.AdminWeb** — inline HTML panel admina (brak auth!)

### Frontend / Client
- **client-unity/** — Unity 2D mobile klient (poza scopem backendu)

### Infrastruktura
- Docker Compose: PostgreSQL 16 + Redis 7
- Migracje SQL: 21 plików (001–021), schemat istnieje ale **nie jest używany** (InMemory store)
- Health checks: skonfigurowane w docker-compose

---

## 2. Lista modułów

| Moduł | Opis | Stan |
|-------|------|------|
| Session | Tworzenie/zarządzanie sesjami gry | Demo-only, InMemory |
| Lobby | Zarządzanie lobby 4-graczy | Demo-only |
| BotDirector | AI fallback dla brakujących graczy | Zaimplementowany |
| CityMap | Mapa miasta, dispatch | Demo data |
| OperationsBoard | Tablica incydentów | Demo data |
| RoundRuntime | Czas rundy, live deltas | Demo data |
| MissionFlow | Przepływ misji (briefing→raport) | Demo data |
| QuickPlay | Szybka rozgrywka | Demo data |
| ReferenceData | Dane referencyjne z JSON | Content-driven |
| ContentValidation | Walidacja bundli JSON | Działa |
| AdminWeb | Panel administracyjny | **BRAK AUTH** |

---

## 3. Lista ryzyk bezpieczeństwa

### 🔴 KRYTYCZNE

| ID | Ryzyko | Lokalizacja | Opis |
|----|--------|-------------|------|
| SEC-01 | **Brak autoryzacji na wszystkich endpointach** | `appsettings.json:Security:RequireAuth=false` | Wszystkie API endpoints są publiczne |
| SEC-02 | **Hardcoded JWT signing key** | `appsettings.json` line 21 | `"dev-only-signing-key-change-me-to-32-plus-chars"` w repo |
| SEC-03 | **Admin panel bez auth** | `Alarm112.AdminWeb/Program.cs` | Panel admina dostępny publicznie bez logowania |
| SEC-04 | **Dev token endpoint otwarty** | `Program.cs` line 192, `appsettings.json` `EnableDevTokenEndpoint:true` | Każdy może wygenerować token dla dowolnej roli |
| SEC-05 | **Hardcoded DB credentials** | `infra/docker-compose.yml` | `postgres/postgres` w źródle |

### 🟠 WYSOKIE

| ID | Ryzyko | Lokalizacja | Opis |
|----|--------|-------------|------|
| SEC-06 | **Brak walidacji wejścia na DTO** | `Alarm112.Contracts/` | Zero atrybutów walidacyjnych, brak FluentValidation |
| SEC-07 | **Brak security headers** | `Program.cs` | Brak X-Frame-Options, CSP, HSTS, X-Content-Type-Options |
| SEC-08 | **CORS AllowCredentials + AllowAnyHeader** | `Program.cs` lines 88-90 | Ryzyko CSRF w kombinacji z credentials |
| SEC-09 | **Swagger UI dostępny w produkcji** | `Program.cs` lines 153-154 | Brak środowiskowej blokady Swagger |
| SEC-10 | **Rate limiter niezastosowany do endpointów** | `Program.cs` | Limiter zarejestrowany ale niepodpięty pod endpointy (`RequireRateLimiting`) |
| SEC-11 | **Brak Redis auth** | `docker-compose.yml` | Redis bez hasła, port 6379 wystawiony |

### 🟡 ŚREDNIE

| ID | Ryzyko | Lokalizacja | Opis |
|----|--------|-------------|------|
| SEC-12 | **AllowedHosts: "*"** | `appsettings.json` line 8 | Brak ograniczeń nagłówka Host |
| SEC-13 | **InMemory store — brak persystencji** | `InMemorySessionStore.cs` | Dane tracone przy restarcie |
| SEC-14 | **Brak audytu logowania** | All services | Brak logowania operacji biznesowych |
| SEC-15 | **Brak CSRF na POST endpoints** | `Program.cs` | Dotyczy cookie-based flow |
| SEC-16 | **Hub bez auth** | `SessionHub.cs` | SignalR hub nie wymaga auth |
| SEC-17 | **PayloadJson: raw string execution** | `SessionService.cs` | JSON.Parse bez limitu rozmiaru |

---

## 4. Lista brakujących walidacji

| Klasa DTO | Brakujące walidacje |
|-----------|---------------------|
| `SessionActionDto` | `SessionId` max length, `ActorId` max length, `Role` enum check, `ActionType` enum whitelist, `PayloadJson` max size |
| `DispatchCommandDto` | `IncidentId` pattern, `UnitId` pattern, `ActorRole` enum check |
| `QuickPlayStartRequestDto` | Brak walidacji trybu, missionId |
| `RoutePreviewRequestDto` | Brak walidacji współrzędnych |
| `SharedActionDto` | Brak walidacji ID i type |
| `DevTokenRequest` | `Role` już sprawdzany ręcznie (OK), `Subject` bez limitu długości |

---

## 5. Lista brakujących testów

| Kategoria | Brakuje |
|-----------|---------|
| Testy negatywne | Brak — tylko happy path |
| Testy bezpieczeństwa | Żadne (brak tokena, zły token, brak roli) |
| Testy walidacji | Żadne (złe DTO, null, oversized) |
| Testy granic | Brak |
| E2E Playwright | Brak (folder `tests/e2e/` pusty) |
| Testy serwisów domenowych | Brak unit testów dla SessionService, BotDirector |
| Testy InMemorySessionStore | Brak |
| SignalR testy | Brak |
| Admin panel UI testy | Brak |

---

## 6. Lista problemów UX/UI

| Element | Problem |
|---------|---------|
| Admin panel | Brak auth (login screen) |
| Admin panel | Brak prawdziwych danych live (mockowane) |
| Admin panel | Brak obsługi błędów w JS (fetch failure) |
| Swagger | Dostępny publicznie na produkcji |
| API errors | Brak spójnego formatu ProblemDetails na wszystkich ścieżkach błędów |
| Admin panel | Brak CSRF ochrony dla akcji |

---

## 7. Lista problemów wydajnościowych

| Problem | Lokalizacja |
|---------|-------------|
| BotTickHostedService — hardcoded `DEMO112` session | `Program.cs` line 953 |
| Brak pagination na endpointach listy | Wszystkie GET |
| Brak cache na reference data | `ReferenceDataService` |
| InMemory store — brak limitów rozmiaru | `InMemorySessionStore` |
| 28 serwisów Singleton — brak lazy init | `Program.cs` |

---

## 8. Lista problemów DevOps / deployment

| Problem | Opis |
|---------|------|
| .NET 10 **preview** w Dockerfile | `mcr.microsoft.com/dotnet/aspnet:10.0-preview` — nie-prod |
| Brak CI/CD pipeline | Folder `ci/` pusty, brak GitHub Actions |
| Docker non-root user | Brak `USER app` w Dockerfile |
| Brak `.env.production` przykładu | Tylko `.env.example` z portami |
| Brak `COPY .env.example .env` | Nowy developer może nie wiedzieć co ustawić |
| Redis bez TLS i bez auth | Otwarte w sieci lokalnej |

---

## 9. Priorytety: Krytyczne / Wysokie / Średnie / Niskie

```
KRYTYCZNE (fix natychmiastowy):
  SEC-01 RequireAuth=false
  SEC-02 Hardcoded JWT key
  SEC-03 Admin panel brak auth
  SEC-04 Dev token endpoint otwarty
  SEC-05 Hardcoded DB credentials

WYSOKIE (fix przed release):
  SEC-06 Brak walidacji DTO
  SEC-07 Brak security headers
  SEC-08 CORS misconfiguration
  SEC-09 Swagger w prod
  SEC-10 Rate limiter niepodpięty

ŚREDNIE (backlog):
  SEC-11 Redis bez auth
  SEC-12 AllowedHosts: "*"
  SEC-13 InMemory store
  SEC-14 Brak audit log
  SEC-15 CSRF

NISKIE (tech debt):
  SEC-16 Hub bez auth
  SEC-17 PayloadJson raw parse
```
