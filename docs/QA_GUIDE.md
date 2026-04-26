# QA_GUIDE.md — przewodnik QA

## Główne bramki jakości

```text
tools\verify.ps1
  -> dotnet restore
  -> dotnet build
  -> dotnet test
  -> tools\smoke-v26.ps1
  -> tools\content-verify.ps1
```

Dodatkowo:

- `tools\docker-verify.ps1` — weryfikacja Docker stacka
- `tests\e2e` — Playwright dla API/AdminWeb

---

## Lokalna walidacja

```powershell
dotnet build Alarm112.sln
dotnet test Alarm112.sln
.\tools\verify.ps1
```

---

## Health contracts

- `GET /health/live` — proces żyje
- `GET /health/ready` — aplikacja gotowa do ruchu
- `GET /health` — alias readiness dla prostszych probe/integracji

`/health/ready` i `/health` zwracają `503`, gdy brakuje krytycznych katalogów/bundli albo store nie jest gotowy.

---

## Co sprawdza smoke-v26

- obecność kluczowych plików v26,
- jeśli API działa lokalnie lub ustawisz `SMOKE_API_BASE`, także:
  - `/health/live`
  - `/health/ready`
  - `/api/content/validate`

Przykład:

```powershell
$env:SMOKE_API_BASE = "http://localhost:5080"
.\tools\smoke-v26.ps1
```

---

## Co sprawdza content validation

`GET /api/content/validate` oraz `ContentValidationService` w testach walidują:

- katalogi `reference`, `config`, `content`, `ui`, `art`, `audio`
- krytyczne bundla startowe
- składnię JSON dla wszystkich wykrytych plików w tych katalogach

---

## Najważniejsze zestawy testowe

- `HealthEndpointTests`
- `ContentEndpointTests`
- `SessionEndpointTests`
- `SessionServiceTests`
- `Security*Tests`
- `AdminDashboardEndpointTests`
- `ContentValidationServiceTests`
- `BotDirectorTests`

---

## Kontrakt retry / idempotencji dla akcji sesji

- ponowne wyslanie tej samej akcji z tym samym `CorrelationId` nie moze zmienic stanu drugi raz,
- replay powinien zwrocic sukces z flaga `duplicate`,
- realtime envelope ma byc emitowany tylko dla pierwszego, skutecznego przetworzenia akcji,
- snapshoty demo i quickplay powinny pozostawac w kanonicznych statusach gameplay (`pending`, `dispatched`, `escalated`, `resolved`, `available`).

---

## Ręczne sanity checks

```powershell
Invoke-RestMethod http://localhost:5080/health/live
Invoke-RestMethod http://localhost:5080/health/ready
Invoke-RestMethod -Method POST http://localhost:5080/api/sessions/demo
```

Web sanity:

- `http://localhost:5081/`
- `http://localhost:5081/app`
- `http://localhost:5081/admin`
