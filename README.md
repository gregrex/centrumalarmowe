# 112: Centrum Alarmowe — v26

Repo zawiera backend, publiczny landing page, demo dashboard gracza, panel operacyjny AdminWeb, content-driven dane JSON, testy i infrastrukturę pomocniczą dla mobilnej gry 2D real-time dispatch/management.

## Co jest w repo

- `src\Alarm112.Api` — minimal API + SignalR
- `src\Alarm112.AdminWeb` — publiczny web surface (`/`, `/app`) + panel operacyjny admina (`/admin`)
- `src\Alarm112.Application` — logika aplikacyjna i serwisy content-driven
- `src\Alarm112.Infrastructure` — `InMemorySessionStore` i `PostgresSessionStore`
- `data\` — bundla JSON dla contentu, UI, audio, art i reference data
- `tests\Alarm112.Api.Tests` — testy .NET
- `tests\e2e` — Playwright dla API i AdminWeb
- `infra\docker-compose.yml` — stack: API, AdminWeb, PostgreSQL, Redis
- `infra\docker-compose.proxy.yml` + `infra\Caddyfile` — opcjonalny gateway/reverse proxy
- `tools\run-migrations.ps1` — ręczne uruchamianie śledzonych migracji SQL

## Aktualny status

Projekt jest w stanie **showcase / demo-ready backend pack**:

- build i testy przechodzą,
- API ma auth, RBAC, CORS, rate limiting i health endpoints,
- akcje `POST /api/sessions/{sessionId}/actions` sa idempotentne po `CorrelationId` i nie emituja duplikatow realtime przy retry,
- snapshoty demo/quickplay uzywaja kanonicznych rol (`CallOperator`, `Dispatcher`, `OperationsCoordinator`, `CrisisOfficer`) oraz statusow (`pending`, `dispatched`, `escalated`, `resolved`, `available`),
- AI fallback potrafi nie tylko dispatchowac, ale tez eskalowac i rozwiązywac incydenty w zaleznosci od stanu sesji,
- Docker stack automatycznie aplikuje migracje SQL przez serwis `migrate`,
- AdminWeb daje teraz:
  - publiczny landing page produktu,
  - publiczny dashboard demo dla gracza,
  - chroniony dashboard operacyjny admina,
- repo nie jest jeszcze pełnym produktem końcowym z trwałymi kontami end-user, pełnym self-service dashboardem i kompletnym produkcyjnym frontendem.

## Szybki start

### Lokalnie

```powershell
dotnet build Alarm112.sln
dotnet test Alarm112.sln
dotnet run --project src\Alarm112.Api
```

Endpointy:

- `http://localhost:5080/swagger`
- `http://localhost:5080/health/live`
- `http://localhost:5080/health/ready`

### Pełna lokalna weryfikacja

```powershell
.\tools\verify.ps1
```

### Docker

```powershell
Copy-Item infra\.env.example infra\.env
docker compose -f infra\docker-compose.yml up -d --build
```

Web entry points:

- `http://localhost:5081/` — landing page
- `http://localhost:5081/app` — dashboard demo gracza
- `http://localhost:5081/admin` — dashboard admina

## Admin demo

Lokalny panel admina wymaga env credentials. Przykład:

```powershell
$env:AdminAuth__Username = "admin"
$env:AdminAuth__Password = "AdminDemoPass_12345"
$env:ApiAuth__Jwt__SigningKey = "local-demo-signing-key-32-chars!!"
dotnet run --project src\Alarm112.AdminWeb
```

## Demo access

- **AdminWeb:** `admin / AdminDemoPass_12345` po ustawieniu env jak wyzej
- **Gracz/API (Development):** przez `POST /auth/dev-token`, np. subject `demo-player`, role `Dispatcher`

Przyklad:

```powershell
$body = @{ subject = "demo-player"; role = "Dispatcher" } | ConvertTo-Json
Invoke-RestMethod -Method POST http://localhost:5080/auth/dev-token -ContentType "application/json" -Body $body
```

Repo nie ma jeszcze trwalego systemu kont end-user z rejestracja/loginem i dashboardem uzytkownika.

## Kontrakt akcji sesji

- `SessionActionDto.SessionId` musi zgadzac sie z `{sessionId}` w URL.
- `CorrelationId` pelni role klucza idempotencji dla retry klienta/bota.
- Powtorzenie tej samej akcji z tym samym `CorrelationId` zwraca `200 OK` z `duplicate=true`, ale nie publikuje kolejnego envelope przez SignalR.
- Niepoprawna akcja biznesowa (np. brak `incidentId`, niedostepna jednostka, probna akcja na zlym statusie) wraca jako `400` z opisem przyczyny.

## Najważniejsze dokumenty

- `docs\BUILD.md`
- `docs\RUN_LOCAL.md`
- `docs\RUN_DOCKER.md`
- `docs\DEPLOY.md`
- `docs\QA_GUIDE.md`
- `docs\00_AUDYT_STARTOWY.md`
- `docs\03_PLAN_NAPRAWCZY.md`

## Najważniejsze ograniczenia

- wiele przepływów nadal bazuje na demo/showcase data,
- klient Unity nie jest jeszcze kompletny produkcyjnie,
- migracje DB nie są jeszcze w pełni zautomatyzowane,
- publiczny surface produktu istnieje, ale nadal wymaga dalszego rozwoju do pełnego launch quality.
