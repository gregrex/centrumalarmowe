# RUN_LOCAL.md — uruchomienie lokalne

## Wymagania

- .NET 10 SDK
- opcjonalnie Docker Desktop, jeśli chcesz uruchomić PostgreSQL i Redis

---

## 1. API — szybki start (in-memory)

```powershell
cd C:\projekty\centrumalarmowe
dotnet run --project src\Alarm112.Api
```

Domyślne endpointy:

- API: `http://localhost:5080`
- Swagger: `http://localhost:5080/swagger`
- Liveness: `http://localhost:5080/health/live`
- Readiness: `http://localhost:5080/health/ready`

---

## 2. AdminWeb — lokalnie

AdminWeb wymaga credentials z env:

```powershell
$env:AdminAuth__Username = "admin"
$env:AdminAuth__Password = "AdminDemoPass_12345"
$env:ApiBaseUrl = "http://localhost:5080"
$env:ApiAuth__Jwt__SigningKey = "local-demo-signing-key-32-chars!!"
dotnet run --project src\Alarm112.AdminWeb
```

Panel:

- landing: `http://localhost:5081/`
- dashboard gracza: `http://localhost:5081/app`
- panel admina: `http://localhost:5081/admin`
- login: Basic Auth
- user: `admin`
- password: `AdminDemoPass_12345`

Demo gracza/API w trybie Development:

```powershell
$body = @{ subject = "demo-player"; role = "Dispatcher" } | ConvertTo-Json
Invoke-RestMethod -Method POST http://localhost:5080/auth/dev-token -ContentType "application/json" -Body $body
```

---

## 3. API + infrastruktura lokalna

Najpierw uruchom DB i Redis:

```powershell
Copy-Item infra\.env.example infra\.env
docker compose -f infra\docker-compose.yml up db redis -d
```

Następnie ustaw connection stringi dla API:

```powershell
$env:ConnectionStrings__Main = "Host=localhost;Port=5432;Database=alarm112;Username=alarm112app;Password=CHANGE_ME_strong_password_here"
$env:Redis__Connection = "localhost:6379,password=CHANGE_ME_strong_redis_password"
dotnet run --project src\Alarm112.Api
```

Jeśli chcesz włączyć auth lokalnie:

```powershell
$env:Security__RequireAuth = "true"
$env:Security__Jwt__SigningKey = "local-demo-signing-key-32-chars!!"
dotnet run --project src\Alarm112.Api
```

---

## 4. Szybka weryfikacja

```powershell
Invoke-RestMethod http://localhost:5080/health/live
Invoke-RestMethod http://localhost:5080/health/ready
Invoke-RestMethod -Method POST http://localhost:5080/api/sessions/demo
.\tools\smoke-v26.ps1
```

Pełna lokalna bramka:

```powershell
.\tools\verify.ps1
```

---

## 5. Typowe porty

| Usługa | Port |
|---|---|
| API | 5080 |
| AdminWeb | 5081 |
| PostgreSQL | 5432 |
| Redis | 6379 |

Gdy porty są zajęte:

```powershell
.\tools\find-free-port.ps1
```
