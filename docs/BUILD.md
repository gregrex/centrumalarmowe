# BUILD.md — Instrukcja budowania

## Wymagania wstępne

| Narzędzie | Wersja | Link |
|---|---|---|
| .NET SDK | 10 Preview | https://dotnet.microsoft.com/download/dotnet/10.0 |
| Docker Desktop | 4.x+ | https://www.docker.com/products/docker-desktop |
| Node.js (opcjonalnie, testy E2E) | 20 LTS | https://nodejs.org |

Sprawdź zainstalowaną wersję:
```powershell
dotnet --version   # powinno zwrócić 10.0.x-preview
docker --version
```

---

## Build lokalny (.NET)

### Restore + kompilacja całego solution

```powershell
cd c:\projekty\centrumalarmowe
dotnet restore Alarm112.sln
dotnet build Alarm112.sln --no-restore
```

Oczekiwany wynik: `Build succeeded. 0 Warning(s). 0 Error(s).`

### Build pojedynczego projektu

```powershell
dotnet build src/Alarm112.Api/Alarm112.Api.csproj
```

---

## Testy

### Testy integracyjne API

```powershell
dotnet test tests/Alarm112.Api.Tests/Alarm112.Api.Tests.csproj --logger "console;verbosity=normal"
```

Oczekiwany wynik: `Passed: 24 | Failed: 0 | Skipped: 0`

### Wszystkie testy w solution

```powershell
dotnet test Alarm112.sln
```

---

## Build obrazów Docker

```powershell
cd infra
docker compose build
# lub tylko API:
docker compose build api
```

Build context dla Docker to katalog główny repo (`..` względem `infra/`). W obrazie umieszczane są:
- skompilowane dll-e aplikacji
- katalog `data/` z bundlami JSON

---

## Skrypt pełnej weryfikacji

```powershell
.\tools\verify.ps1
```

Skrypt wykonuje kolejno:
1. `dotnet restore`
2. `dotnet build` (no-restore)
3. `dotnet test`
4. `.\tools\smoke-v26.ps1` — walidacja plików
5. `.\tools\content-verify.ps1` — walidacja JSON bundli

Zwraca `exit 0` gdy wszystkie bramki przeszły, `exit 1` przy pierwszym błędzie.

---

## Projekty w solution

| Projekt | Cel |
|---|---|
| `Alarm112.Domain` | Enumeracje i modele domenowe |
| `Alarm112.Contracts` | DTOs — jedyne obiekty przesyłane między warstwami |
| `Alarm112.Application` | Interfejsy i serwisy (logika biznesowa) |
| `Alarm112.Infrastructure` | Persistence (`InMemorySessionStore`) |
| `Alarm112.Api` | Minimal API endpoints + SignalR Hub |
| `Alarm112.AdminWeb` | Panel admina (Blazor/Minimal API) |

Zależności: `Api → Application → Domain`. `Contracts` jest współdzielone.

---

## Znane problemy

- Jeśli build kończy się błędem `CS2012 (plik zajęty)`, zabij poprzedni proces dotnet:
  ```powershell
  Get-Process dotnet | Stop-Process -Force
  ```
- Przy pierwszym uruchomieniu restore może pobrać ~300 MB pakietów NuGet.
