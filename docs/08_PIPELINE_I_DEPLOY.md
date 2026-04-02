# 08 — Pipeline i Deploy: 112 Centrum Alarmowe

---

## Architektura CI/CD

```
Push/PR
  └─► GitHub Actions: ci.yml
       ├── restore
       ├── build
       ├── test (unit + integration)
       ├── lint (dotnet-format)
       └── content-verify

Merge to main
  └─► GitHub Actions: docker.yml
       ├── docker build (api + admin)
       ├── docker push → registry
       ├── smoke tests (tools/smoke-v26.ps1)
       └── notify
```

---

## GitHub Actions — ci.yml

Plik: `.github/workflows/ci.yml`

```yaml
name: CI

on:
  push:
    branches: [ main, dev ]
  pull_request:
    branches: [ main ]

jobs:
  build-and-test:
    runs-on: ubuntu-latest
    
    steps:
    - name: Checkout
      uses: actions/checkout@v4

    - name: Setup .NET
      uses: actions/setup-dotnet@v4
      with:
        dotnet-version: '10.0.x'
        dotnet-quality: 'preview'

    - name: Restore
      run: dotnet restore Alarm112.sln

    - name: Build
      run: dotnet build Alarm112.sln --no-restore -c Release

    - name: Test
      run: dotnet test Alarm112.sln --no-build -c Release \
             --collect:"XPlat Code Coverage" \
             --results-directory ./coverage

    - name: Content validate
      run: pwsh tools/content-verify.ps1

    - name: Upload coverage
      uses: codecov/codecov-action@v4
      with:
        directory: ./coverage
```

---

## GitHub Actions — docker.yml

Plik: `.github/workflows/docker.yml`

```yaml
name: Docker Build & Push

on:
  push:
    branches: [ main ]
    tags: [ 'v*' ]

env:
  REGISTRY: ghcr.io
  IMAGE_API: ${{ github.repository }}/alarm112-api
  IMAGE_ADMIN: ${{ github.repository }}/alarm112-admin

jobs:
  docker:
    runs-on: ubuntu-latest
    permissions:
      contents: read
      packages: write

    steps:
    - uses: actions/checkout@v4

    - name: Log in to registry
      uses: docker/login-action@v3
      with:
        registry: ${{ env.REGISTRY }}
        username: ${{ github.actor }}
        password: ${{ secrets.GITHUB_TOKEN }}

    - name: Build and push API
      uses: docker/build-push-action@v5
      with:
        context: .
        file: src/Alarm112.Api/Dockerfile
        push: true
        tags: ${{ env.REGISTRY }}/${{ env.IMAGE_API }}:latest

    - name: Build and push Admin
      uses: docker/build-push-action@v5
      with:
        context: .
        file: src/Alarm112.AdminWeb/Dockerfile
        push: true
        tags: ${{ env.REGISTRY }}/${{ env.IMAGE_ADMIN }}:latest

    - name: Smoke test
      run: |
        docker compose -f infra/docker-compose.yml up -d
        sleep 10
        pwsh tools/smoke-v26.ps1
        docker compose -f infra/docker-compose.yml down
```

---

## Dockerfile — zmiany wymagane

### Alarm112.Api/Dockerfile — poprawki

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
# TODO: zmienić na 10.0 gdy stable

# Security: non-root user
RUN groupadd -r appgroup && useradd -r -g appgroup appuser

WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src
# ... (bez zmian) ...
RUN dotnet publish ... -o /app/out

FROM base AS final
WORKDIR /app
COPY --from=build /app/out .
COPY data/ /app/data/

# Security: non-root
USER appuser

ENV ContentBundles__DataRoot=/app/data
HEALTHCHECK --interval=10s --timeout=5s --start-period=30s --retries=5 \
  CMD wget -qO- http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Alarm112.Api.dll"]
```

---

## Health Endpoints

### Istniejący
```
GET /health → { ok: true, service: "Alarm112.Api", version: "v26" }
```

### Rozszerzony (do implementacji)
```
GET /health/ready → sprawdź: data bundles loaded, session store ready
GET /health/live  → sprawdź: process alive
GET /health/detail → { ok, db: "connected|disconnected", redis: "connected|disconnected" }
```

---

## Zmienne środowiskowe — pełna lista

| Zmienna | Wymagana | Default | Opis |
|---------|----------|---------|------|
| `ASPNETCORE_ENVIRONMENT` | Tak | `Development` | Środowisko runtime |
| `ASPNETCORE_URLS` | Tak | `http://+:8080` | Bind URL |
| `ContentBundles__DataRoot` | Tak | `../../data` | Ścieżka do data/ |
| `Security__RequireAuth` | Prod | `false` | Włącz wymuszenie auth |
| `Security__EnableDevTokenEndpoint` | Prod | `true` | Wyłącz w prod! |
| `Security__Jwt__SigningKey` | Prod | (dev default) | Klucz JWT — ZMIEŃ! |
| `Security__Jwt__Issuer` | Nie | `Alarm112.Api` | JWT issuer |
| `Security__Jwt__Audience` | Nie | `Alarm112.Client` | JWT audience |
| `ConnectionStrings__Main` | DB mode | (brak) | PostgreSQL conn string |
| `Redis__Connection` | Redis mode | (brak) | Redis conn string |
| `Cors__AllowedOrigins` | Nie | `http://localhost:*` | Dozwolone origins |
| `ApiBaseUrl` | AdminWeb | `http://localhost:5080` | URL API dla AdminWeb |

---

## Deployment checklist

Przed deploymentem na produkcję:

```
Infrastructure:
  [ ] PostgreSQL z dedykowanym userem
  [ ] Redis z hasłem
  [ ] TLS/HTTPS (reverse proxy lub certbot)
  [ ] Firewall: tylko porty 80/443 publiczne
  [ ] Backup bazy skonfigurowany

Application:
  [ ] ASPNETCORE_ENVIRONMENT=Production
  [ ] Security__RequireAuth=true
  [ ] Security__EnableDevTokenEndpoint=false
  [ ] Security__Jwt__SigningKey ustawiony (silny, losowy)
  [ ] ConnectionStrings__Main ustawiony
  [ ] Swagger wyłączony (lub za auth)
  [ ] AllowedHosts ustawiony na domenę

Docker:
  [ ] Non-root user w Dockerfile
  [ ] .NET stable (nie preview) w Dockerfile
  [ ] Image scanning (trivy / snyk)
  [ ] docker-compose.override.yml z prawdziwymi hasłami (gitignored)
```

---

## Smoke tests — weryfikacja po deploy

```powershell
# tools/smoke-prod.ps1
$base = $env:API_URL ?? "http://localhost:5080"

# Health check
$resp = Invoke-RestMethod "$base/health"
if (!$resp.ok) { throw "Health check FAILED" }

# Swagger wyłączony w prod
try {
    $sw = Invoke-WebRequest "$base/swagger" -ErrorAction Stop
    if ($sw.StatusCode -eq 200) { Write-Warning "⚠ Swagger is accessible in prod!" }
} catch { Write-Host "✅ Swagger correctly blocked" }

# Dev token wyłączony w prod
try {
    $tok = Invoke-RestMethod "$base/auth/dev-token" -Method POST -Body '{"Role":"Dispatcher"}' -ContentType "application/json" -ErrorAction Stop
    Write-Error "❌ Dev token endpoint accessible in prod!"
} catch { Write-Host "✅ Dev token endpoint correctly blocked" }

Write-Host "✅ Smoke tests passed"
```
