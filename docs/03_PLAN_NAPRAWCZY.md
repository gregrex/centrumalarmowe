# 03 — Plan Naprawczy: 112 Centrum Alarmowe

> Iteracyjny plan doprowadzenia projektu do stanu produkcyjnego.

---

## Etap 1: Krytyczne bezpieczeństwo

**Cel:** Wyeliminować wszystkie ryzyka krytyczne (SEC-01 do SEC-05).

### Zadania

| # | Zadanie | Plik(i) | Priorytet |
|---|---------|---------|-----------|
| 1.1 | Utworzyć `appsettings.Production.json` z `RequireAuth=true`, `EnableDevTokenEndpoint=false` | `src/Alarm112.Api/` | 🔴 |
| 1.2 | Usunąć hardcoded JWT key z `appsettings.json` → user-secrets + env var | `appsettings.json` | 🔴 |
| 1.3 | Dodać Basic Auth middleware na AdminWeb | `src/Alarm112.AdminWeb/Program.cs` | 🔴 |
| 1.4 | Dodać Security Headers middleware | `src/Alarm112.Api/Program.cs` | 🔴 |
| 1.5 | Przenieść DB credentials do `.env` (gitignored) | `infra/docker-compose.yml` | 🔴 |
| 1.6 | Zablokować Swagger poza Development | `src/Alarm112.Api/Program.cs` | 🟠 |
| 1.7 | Podpiąć rate limiter do endpointów POST | `src/Alarm112.Api/Program.cs` | 🟠 |
| 1.8 | Dodać `appsettings.Development.json` z dev overrides | `src/Alarm112.Api/` | 🟠 |

---

## Etap 2: Walidacje frontend + backend

**Cel:** Każdy DTO ma pełną walidację. Każdy endpoint zwraca ProblemDetails na błędach.

### Zadania

| # | Zadanie | Plik(i) |
|---|---------|---------|
| 2.1 | Dodać DataAnnotations do `SessionActionDto` | `Alarm112.Contracts/` |
| 2.2 | Dodać DataAnnotations do `DispatchCommandDto` | `Alarm112.Contracts/` |
| 2.3 | Dodać DataAnnotations do `QuickPlayStartRequestDto` | `Alarm112.Contracts/` |
| 2.4 | Dodać DataAnnotations do `RoutePreviewRequestDto` | `Alarm112.Contracts/` |
| 2.5 | Dodać DataAnnotations do `SharedActionDto` | `Alarm112.Contracts/` |
| 2.6 | Dodać walidację biznesową w `SessionService` | `Alarm112.Application/` |
| 2.7 | Ujednolicić format błędów (ProblemDetails) | `Program.cs` |
| 2.8 | Dodać walidację `PayloadJson` max size (1KB) | `SessionService.cs` |

---

## Etap 3: Testy jednostkowe i integracyjne

**Cel:** Pokrycie testami kluczowych metod i endpointów.

### Zadania

| # | Zadanie | Plik(i) |
|---|---------|---------|
| 3.1 | Unit testy `SessionService` (dispatch, escalate, resolve) | `tests/Alarm112.Api.Tests/` |
| 3.2 | Unit testy `InMemorySessionStore` | `tests/Alarm112.Api.Tests/` |
| 3.3 | Testy walidacji DTO (negatywne przypadki) | `tests/Alarm112.Api.Tests/` |
| 3.4 | Testy bezpieczeństwa: brak tokena → 401 | `tests/Alarm112.Api.Tests/` |
| 3.5 | Testy bezpieczeństwa: zły token → 401 | `tests/Alarm112.Api.Tests/` |
| 3.6 | Testy bezpieczeństwa: wygasły token → 401 | `tests/Alarm112.Api.Tests/` |
| 3.7 | Testy rate limiting → 429 | `tests/Alarm112.Api.Tests/` |
| 3.8 | Testy security headers | `tests/Alarm112.Api.Tests/` |
| 3.9 | Testy idempotentności akcji (duplicate correlationId) | `tests/Alarm112.Api.Tests/` |

---

## Etap 4: Testy E2E i scenariusze użytkownika

**Cel:** Playwright testy dla admin panelu i kluczowych przepływów.

### Zadania

| # | Zadanie |
|---|---------|
| 4.1 | Setup Playwright w `tests/e2e/` |
| 4.2 | Test: admin panel ładuje się poprawnie |
| 4.3 | Test: health endpoint zwraca OK |
| 4.4 | Test: tworzenie sesji demo → dispatch → raport |
| 4.5 | Test: próba dostępu do API bez auth → 401 |
| 4.6 | Test: brakujący sessionId → 404 |

---

## Etap 5: Refaktoryzacja architektury

**Cel:** Usunięcie długu technicznego, lepsza czytelność kodu.

### Zadania

| # | Zadanie | Uzasadnienie |
|---|---------|-------------|
| 5.1 | Przenieść `BotTickHostedService` z `Program.cs` do `Application/Services/` | Separation of concerns |
| 5.2 | Przenieść `DevTokenRequest` do Contracts | Brak logiki w Program.cs |
| 5.3 | Wydzielić endpoint groups do osobnych plików | Program.cs ma 962 linii |
| 5.4 | Dodać `appsettings.Production.json` z overrides | Środowiskowa konfiguracja |
| 5.5 | Podłączyć PostgreSQL (EF Core lub Dapper) | Zastąpić InMemory store |
| 5.6 | Dodać ILogger do serwisów | Audit trail |

---

## Etap 6: Redesign UI/UX web (Admin Panel)

**Cel:** Panel admina z auth, live data, profesjonalnym wyglądem.

### Zadania

| # | Zadanie |
|---|---------|
| 6.1 | Ekran logowania Basic Auth w AdminWeb |
| 6.2 | Live data z API (prawdziwe fetch, nie mockowane) |
| 6.3 | Obsługa błędów w JS (fetch failure → error state) |
| 6.4 | Responsive layout (mobile-friendly) |
| 6.5 | Session list view z filtrowaniem |
| 6.6 | Bot status panel (live) |

---

## Etap 7: Redesign UI/UX mobile (Unity)

**Cel:** Zgodność z design system, mobile-first.

### Zadania

| # | Zadanie |
|---|---------|
| 7.1 | Integracja design-tokens.css z Unity (font size, kolory) |
| 7.2 | HUD layout per rola |
| 7.3 | Ekrany onboarding |
| 7.4 | Error states w UI |

---

## Etap 8: Zabezpieczenie bazy, konfiguracji i sekretów

| # | Zadanie |
|---|---------|
| 8.1 | `.env.production.example` z opisem wymaganych secrets |
| 8.2 | `docker-compose.override.yml` dla dev (gitignored) |
| 8.3 | Redis auth (`requirepass`) |
| 8.4 | PostgreSQL: dedykowany user z min uprawnieniami |
| 8.5 | Podłączyć PostgreSQL do aplikacji |
| 8.6 | Uruchomić migracje w CI/CD |

---

## Etap 9: CI/CD, Docker, health checks, smoke tests

| # | Zadanie |
|---|---------|
| 9.1 | GitHub Actions: `ci.yml` (restore → build → test → lint) |
| 9.2 | GitHub Actions: `docker.yml` (build → push → smoke) |
| 9.3 | Dockerfile: non-root user (`USER app`) |
| 9.4 | Dockerfile: zmiana z `10.0-preview` na `10.0` |
| 9.5 | Health endpoint rozszerzony (db, redis status) |
| 9.6 | Smoke tests w CI (istniejące `tools/smoke-v26.ps1`) |

---

## Etap 10: Finalne porządki i release readiness

| # | Zadanie |
|---|---------|
| 10.1 | Aktualizacja README z instrukcją produkcyjną |
| 10.2 | Release readiness checklist (docs/10_RELEASE_READINESS_CHECKLIST.md) |
| 10.3 | Przegląd backlogu refaktoryzacji |
| 10.4 | Raport końcowy audytu |
| 10.5 | CHANGELOG aktualizacja |

---

## Harmonogram (orientacyjny, bez dat)

```
Etap 1 → Etap 2 → Etap 3 (równolegle) → Etap 4
                ↘ Etap 5 (równolegle z 3/4)
                ↘ Etap 6+7 (równolegle z 5)
Etap 8 → Etap 9 → Etap 10
```
